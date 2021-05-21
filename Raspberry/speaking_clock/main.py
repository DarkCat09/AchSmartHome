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
import core_functions as fn
import api_functions as apifn
from user_prefs import Prefs

# Инициализация моего класса Preferences
prefs = Prefs()


# Настраиваем распознавание речи, плеер, Википедию, API ЯМузыки...
# Если какой-то из них отключен, он просто не используется
# во избежание исключений, которые в этом блоке я не хочу обрабатывать

if ('speech_recognition' in sys.modules):
	recog = sr.Recognizer()
	mic = sr.Microphone()

if ('pygame' in sys.modules):
	mixer.pre_init(44100, -16, 2, 1024)
	mixer.init()

if ('wikipedia' in sys.modules):
	wikipedia.set_lang(prefs.language)

if ('yandex_music' in sys.modules):
	ymcl = yandex_music.Client.from_credentials(prefs.ymcreds_username, prefs.ymcreds_password)

print('Initialize OK!')


# Основной цикл
try:
	while True:

		if (mixer.music.get_busy() and prefs.stop_recog_on_music):
			pass
		else:
			
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
						fn.say(fn.get_current_time())

					# Определяем город
					if fn.user_said(phrase, ['местоположение','текущий город','где я нахожусь']):

						# Получаем IP-адрес
						curip = requests.get('http://ifconfig.me/ip').text

						# Определяем город через API DaData
						fn.say(apifn.get_dadata_city_by_ip(curip))

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
					if fn.user_said(phrase, ['погода в','погода на']):
						
						# Итак, берём название города.
						# У нас может быть не только погода в Ульяновске,
						# но и на Бали (предлог меняется), поэтому уточняем,
						# что сказал пользователь.
						# Если "погода в" не найдена (кол-во вхождений = -1),
						# значит мы имеем дело с погодой "на"
						after_weather_cmd = phrase.lower().find('погода в')
						if (after_weather_cmd < 0):
							# +1, который в конце, это наш "любимый" пробел
							after_weather_cmd = phrase.lower().find('погода на') + 1
						else:
							# так же плюсуем пробел, если погода "в"
							after_weather_cmd += 1

						# В итоге у нас есть индекс для среза строки
						# от основной команды до конца.
						# А теперь ищем указание времени погоды, если есть
						# (на завтра, через час), и обрабатываем
						before_time_weather_cmd = -1

						# Получаем город в Именительном падеже (nc = Nominative Case)
						# через API htmlweb.ru
						# Если сервис не сможет выдать результат, то
						# нулевого элемента просто не будет, и скрипт
						# перейдёт к блоку except в результате исключения OutOfRange
						# (код перенесён в api_functions)
						city_nc_name = apifn.inflect((phrase[after_weather_cmd:].replace(' ','-')), 'им,лок')

						# Получаем lat/lon и погоду через API OpenWeatherMap
						owm_api_token = '356c9ae6724b02017df79fff753f5d5e'
						owm_api_result = requests.get(
							'https://api.openweathermap.org/data/2.5/weather?q=' +
							city_nc_name + '&units=metric&lang=ru&appid=' + owm_api_token)
						
						city_latitude = owm_api_result['coord']['lat']
						city_longitude = owm_api_result['coord']['lon']

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
						fn.play_file('story.mp3', playpause_key=prefs.key_playpause)

					# Музыка
					if fn.user_said(phrase, ['включи песню','запусти песню','включи музыку','запусти музыку']):

						#fn.say('Запускаю')

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
							fn.play_file('music.mp3', playpause_key=prefs.key_playpause)

					# Кулинарные рецепты
					if fn.user_said(phrase, ['рецепт']):
						
						# Берём название рецепта, которое находится после слова "рецепт".
						# Та же схема: поиск вхождения основной команды "рецепт",
						# смещаемся на столько же символов, сколько в команде (6 букв)
						# и прибавляем пробел, который обязательно будет (итого: 7 символов).
						dish_name = phrase[phrase.find('рецепт')+7:]

						# Пользователь мог сказать название блюда
						# как в Именительном, так и в Родительном падеже
						# (например, "рецепт цветаевского пирога" - Р. п.)
						# Сервису HtmlWeb всё равно, в каком падеже
						# я пихну ему словосочетание, поэтому подстрахуемся
						dish_correct_name = apifn.inflect(dish_name)

						# А теперь воспользуемся моей удобной (но огромной)
						# функцией парсинга рецептов
						fn.say(apifn.parse_recipe(dish_correct_name))

				except Exception as ex:
					print("Error happened!", \
					fn.get_current_time(), \
					str(ex), sep='\n')

except KeyboardInterrupt:
	print("Stopped by keyboard!", \
	fn.get_current_time(), \
	sep='\n')

finally:
	mixer.quit()
	del prefs
