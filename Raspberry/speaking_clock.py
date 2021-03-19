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

import os
import time
import datetime
import json
import requests
import wikipedia
import speech_recognition as sr
from gtts import gTTS
from pygame import mixer

# Настраиваем распознавание речи и плеер
recog = sr.Recognizer()
mic = sr.Microphone()
mixer.pre_init(44100, -16, 2, 1024)
mixer.init()
wikipedia.set_lang('ru')

# Функция для поиска команды в выводе распознователя:
# Вывод SR, варианты команды
def user_said(sr_out, cmds):
	srlower = sr_out.lower()
	for cmd in cmds:
		if (srlower.find(cmd) > -1):
			return True
	return False

# Функция для синтезации речи:
def say(text_to_synth):
	# Сохраняем файл с синтезированной Гуглом речью
	gTTS(text=text_to_synth, lang='ru').save('tts.mp3')
	time.sleep(0.01)
	# Подгружаем и запускаем файлик
	mixer.music.load('tts.mp3')
	mixer.music.play()
	# Ждём, пока tts всё скажет...
	while mixer.music.get_busy():
		time.sleep(0.01)
	# Останавливаем воспроизведение
	mixer.music.stop()
	time.sleep(0.01)
	# Удаляем файл
	if os.path.exists('tts.mp3'):
		os.remove('tts.mp3')
	time.sleep(0.01)

# Основной цикл
try:
	while True:
		with mic as audio_file:

			# Записываем речь
			recog.adjust_for_ambient_noise(audio_file)
			audio = recog.listen(audio_file)

			try:

				# Распознаём с помощью сервисов Google
				phrase = recog.recognize_google(audio, language='ru-RU')
				print(phrase)

				# Приветствие
				if user_said(phrase, ['привет','здравствуй','доброе утро','добрый день','добрый вечер']):
					say('Здравствуйте!')

				# Сколько время?
				if user_said(phrase, ['сколько время','сколько сейчас время', 'только сейчас время']):
					# слово "только" добавлено из-за
					# некорректности распознавания в некоторых случаях
					now = datetime.datetime.now()
					say(str(now.strftime('%H:%M')))

				# Определяем город
				if user_said(phrase, ['местоположение','текущий город','где я нахожусь']):

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
						say('Текущее местоположение: '+country+', '+city_type+' '+city_name)

				# Поиск в Википедии
				if user_said(phrase, ['википедия','википедии']):

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
					say(wikipedia.summary(phrase[phrase.find('википеди'+wiki_ending)+10:]))

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
