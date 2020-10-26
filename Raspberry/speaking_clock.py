## Speaking clock for AchSmartHome
##
## Copyright © 2020 Чечкенёв Андрей
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
import speech_recognition as sr
from gtts import gTTS
from pygame import mixer

recog = sr.Recognizer()
mic = sr.Microphone()

try:
	while True:
		with mic as audio_file:
			recog.adjust_for_ambient_noise(audio_file)
			audio = recog.listen(audio_file)
			try:
				phrase = recog.recognize_google(audio, language='ru-RU')
				print(phrase)
				if (phrase.lower().find("сколько время") > -1) or \
				(phrase.lower().find("сколько сейчас время") > -1) or \
				(phrase.lower().find("только сейчас время") > -1):
					# слово "только" добавлено из-за
					# некорректности распознавания в некоторых случаях
					mixer.init()
					now = datetime.datetime.now()
					gTTS(text=str(now.strftime('%H:%M')), lang='ru').save("time.mp3")
					mixer.music.load("time.mp3")
					mixer.music.play()
					while mixer.music.get_busy():
						time.sleep(0.01)
					mixer.music.stop()
					mixer.quit()
					time.sleep(0.01)
					if os.path.exists("time.mp3"):
						os.remove("time.mp3")
					time.sleep(0.01)
			except Exception as ex:
				print("")
				print("Error happened!", \
				str(datetime.datetime.now().strftime('%H:%M:%S')), \
				str(ex), sep='\n')
				print("")
except KeyboardInterrupt:
	print("")
	print("Stopped by keyboard!", \
	str(datetime.datetime.now().strftime('%H:%M:%S')), \
	sep='\n')
	print("")
