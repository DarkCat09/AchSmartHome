## --------------------------------------------------------------------
## Speaking clock for AchSmartHome
##
## Copyright © 2020-2021 Чечкенёв Андрей
##
## This program is free software: you can redistribute it and/or modify
## it under the terms of the GNU General Public License as published by
## the Free Software Foundation, either version 2 of the License, or
## (at your option) any later version.
##
## This program is distributed in the hope that it will be useful,
## but WITHOUT ANY WARRANTY; without even the implied warranty of
## MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
## GNU General Public License for more details.
##
## You should have received a copy of the GNU General Public License
## along with this program.  If not, see <https://www.gnu.org/licenses/>.
## ----------------------------------------------------------------------


# Модули.
# Закомментируйте строку с импортом,
# если какой-то функционал не нужен
# (например, wikipedia или yandex_music)
import os
import sys
import time
import datetime
import keyboard
import sys
import json
import requests
import wikipedia
import yandex_music
import speech_recognition as sr
from gtts import gTTS
from pygame import mixer

# Подгрузка отдельных файлов умных часов
# с функциями и параметрами.
# НЕ ТРОГАТЬ!
import functions as fn
from user_prefs import Prefs

# Инициализация моего класса Preferences
prefs = Prefs()


# Настраиваем распознавание речи, плеер, Википедию, API ЯМузыки...
# Если какой-то из них отключен, он просто не используется
# во избежание исключений, которые в этом блоке я не хочу обрабатывать

if ('speech_recognition' in sys.modules):
	recog = sr.Recognizer()
	mic = sr.Microphone()

mixer_playing = False
if ('pygame' in sys.modules):
	mixer.pre_init(44100, -16, 2, 1024)
	mixer.init()

if ('wikipedia' in sys.modules):
	wikipedia.set_lang(prefs.language)

if ('yandex_music' in sys.modules):
	ymcl = yandex_music.Client.from_credentials(prefs.ymcreds_username, prefs.ymcreds_password)


# Основной цикл
try:
	while True:
		with mic as audio_file:

			# Записываем речь
			recog.adjust_for_ambient_noise(audio_file)
			audio = recog.listen(audio_file)

			try:

				# Распознаём с помощью сервисов Google
				phrase = recog.recognize_google(audio, language=prefs.localization)
				print(phrase)

				# Приветствие
				if fn.user_said(phrase, ['привет','здравствуй','доброе утро','добрый день','добрый вечер']):
					fn.say('Здравствуйте!')

				# Сколько время?
				if fn.user_said(phrase, ['сколько время','сколько сейчас время', 'только сейчас время']):
					# слово "только" добавлено из-за
					# некорректности распознавания в некоторых случаях
					now = datetime.datetime.now()
					fn.say(str(now.strftime('%H:%M')))

				# Определяем город
				if fn.user_said(phrase, ['местоположение','текущий город','где я нахожусь']):

					# Получаем IP-адрес
					curip = requests.get('http://ifconfig.me/ip').text

					# Определяем город через API DaData
					api_url = 'https://suggestions.dadata.ru/suggestions/api/4_1/rs/iplocate/address?ip='+curip+'&language=ru'
					api_token = 'Token cb94fce6082599b4fc7b97c8826c344415a3c3ea'

					api_result = requests.get( \
						api_url, \
						headers={'Accept': 'application/json', 'Authorization': api_token}).text

					api_json_result = json.loads(api_result)['location']

					if (api_json_result != None):
						country = api_json_result['data']['country']
						city_type = api_json_result['data']['city_type_full']
						city_name = api_json_result['data']['city']
						fn.say('Текущее местоположение: '+country+', '+city_type+' '+city_name)

				# Поиск в Википедии
				if fn.user_said(phrase, ['википедия','википедии']):

					# Определяем, что именно сказал пользователь: википедиЯ или (найти в)википедиИ
					wiki_ending = ''
					if (phrase.lower().find('википедия') > -1):
						wiki_ending = 'я'
					elif (phrase.lower().find('википедии') > -1):
						wiki_ending = 'и'

					# Вырезаем из фразы данные о желаемой статье (предположительный заголовок),
					# Берём первый абзац об этом из Википедии и TTS'им.
					# Число 10 = кол-во отсекаемых символов для получения только заголовка
					# (9 букв из слова "википедия" + 1 пробел)
					fn.say(wikipedia.summary(phrase[phrase.find('википеди'+wiki_ending)+10:]))

				# Погода сейчас
				if fn.user_said(phrase, ['погода в']):
					fn.say('Погода в процессе разработки')

				# Аудиосказки для детей!
				if fn.user_said(phrase, ['расскажи сказку', 'сказка']):

					#fn.say('Запускаю')

					# Так же, как и в случае с Википедией, определяем окончание
					story_cmd_ending = ''
					if (phrase.lower().find('сказку') > -1):
						story_cmd_ending = 'у'
					elif (phrase.lower().find('сказка') > -1):
						story_cmd_ending = 'а'

					# Всё тем же способом извлекаем название сказки (6 букв из "сказка" + 1 пробел)
					ymcl.search(phrase[phrase.find('сказк'+story_cmd_ending)+7:], True, 'track').tracks.results[0].download('story.mp3', 'mp3')
					# Запускаем
					fn.play_file('story.mp3')

				if fn.user_said(phrase, ['включи песню','запусти песню','включи музыку','запусти музыку']):

					#say('Запускаю')

					# Узнаём, что именно сказал пользователь
					# (мне лень переписывать user_said)
					song_cmd = ''
					if (phrase.lower().find('песню') > -1):
						song_cmd = 'песню'
					elif (phrase.lower().find('музыку') > -1):
						song_cmd = 'музыку'

					# Проверяем, что пользователь сказал название песни/альбома/исполнителя:
					# Если длина фразы после команды больше двух символов
					# (ожидаемый пробел + мало ли, вдруг recognizer
					#  что-нибудь в конец добавляет),
					# то пытаемся найти музЫку на Яндексе и воспроизвести её
					after_cmd = phrase[phrase.lower().find(song_cmd)+len(song_cmd):]
					if (len(after_cmd) > 2):
						# Пропускаем первый пробел и ищем на ЯМузыке
						ymcl.search(after_cmd[1:], True, 'track').tracks.results[0].download('music.mp3', 'mp3')
						# Запускаем
						fn.play_file('music.mp3')

			except Exception as ex:
				print("Error happened!", \
				str(datetime.datetime.now().strftime('%H:%M:%S')), \
				str(ex), sep='\n')

except KeyboardInterrupt:
	print("Stopped by keyboard!", \
	str(datetime.datetime.now().strftime('%H:%M:%S')), \
	sep='\n')

finally:
	mixer.quit()
	del prefs
