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


# Модули:
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
	mixer_playing = True
	# Ждём, пока tts всё скажет...
	while mixer.music.get_busy():
		time.sleep(0.01)
	# Останавливаем воспроизведение
	mixer.music.stop()
	mixer_playing = False
	time.sleep(0.01)
	# Удаляем файл
	if os.path.exists('tts.mp3'):
		os.remove('tts.mp3')
	time.sleep(0.01)


# Функция-callback для обработки нажатия паузы
def on_pause_down(name):
	print("entered")
	if (name == prefs_key_playpause):
		if (mixer_playing):
			print("paused")
			mixer.music.pause()
		else:
			print("playing")
			mixer.music.unpause()

# Функция для проигрывания MP3-файлов:
def play_file(filename, delete_mp3=True):
	# Проверяем существование файлика
	if os.path.exists(filename):
		# Подгружаем в плеер и запускаем
		mixer.music.load(filename)
		mixer.music.play()
		mixer_playing = True
		# Play/Пауза
		pause_key_hook = None
		if (prefs_key_playpause != None):
			pause_key_hook = keyboard.hook_key(prefs_key_playpause, on_pause_down)
		# Ждём...
		while mixer.music.get_busy():
			time.sleep(0.01)
		# Останавливаем
		if (pause_key_hook != None):
			keyboard.unhook_key(pause_key_hook)
		mixer.music.stop()
		mixer_playing = False
		time.sleep(0.01)
		# Удаляем файл по окончанию, если надо
		if (delete_mp3) and (os.path.exists(filename)):
			os.remove(filename)
		time.sleep(0.01)
