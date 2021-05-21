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
import re
import sys
import time
import datetime
import keyboard
from gtts import gTTS
import pygame
from pygame import mixer

# "Закончилась ли музыка в mixer'е?"
is_music_over = True
# Константа для события конца аудиодорожки
SONG_END = pygame.USEREVENT + 1


# Функция для вывода текущей даты и времени
# в нужном мне формате
def get_current_time():
	return datetime.datetime.now().strftime('%H:%M:%S')


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


# Функция-callback для обработки нажатия паузы
def on_pause_down(keyevent):
	if True:
		if (mixer.music.get_busy()):
			mixer.music.pause()
			print("paused")
		else:
			mixer.music.unpause()
			mixer.music.set_endevent(SONG_END)
			print("playing")

		if (keyboard.is_pressed(keyevent.name)):
				while (keyboard.is_pressed(keyevent.name)):
					time.sleep(0.1)

# Функция для проигрывания MP3-файлов:
def play_file(filename, playpause_key=None, delete_mp3=True):
	# Проверяем существование файлика
	if os.path.exists(filename):
		# Подгружаем в плеер и запускаем
		mixer.music.load(filename)
		mixer.music.play()
		# Событие по завершению аудиодорожки
		mixer.music.set_endevent(SONG_END)
		# Переменная для цикла
		is_music_over = False
		# Play/Пауза
		pause_key_hook = None
		if (playpause_key != None):
			pause_key_hook = keyboard.hook_key(playpause_key, on_pause_down)
			print(f"hook added on key {playpause_key}")
		# Ждём...
		while not is_music_over:
			# Если вдруг в событиях обнаружится, что...
			for event in pygame.event.get():
				# ... песня закончилась, то ...
				if event.type == SONG_END:
					# записываем это и завершаем цикл
					is_music_over = True
			time.sleep(0.01)
		# Останавливаем
		if (pause_key_hook != None):
			keyboard.unhook_key(pause_key_hook)
			print("hook removed")
		mixer.music.stop()
		time.sleep(0.01)
		# Удаляем файл по окончанию, если надо
		if (delete_mp3) and (os.path.exists(filename)):
			os.remove(filename)
		time.sleep(0.01)
