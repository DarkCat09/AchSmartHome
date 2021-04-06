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

class Prefs:

	def __init__(self):

		# ПОЛЬЗОВАТЕЛЬСКИЕ ПАРАМЕТРЫ:

		self.__key_playpause = 'P' # Клавиша для возобновления/приостановки воспроизведения (None, если не используется)

		self.__localization		= 'ru-RU' # Локализация
		self.__language			= 'ru' # Язык

		self.__ymcreds_username 	= 'achtest22@yandex.ru' # Ваш E-Mail на Яндексе
		self.__ymcreds_password 	= '''tEs#t22''' # Пароль от почты

	@property
	def key_playpause(self):
		return self.__key_playpause

	@property
	def localization(self):
		return self.__localization

	@property
	def language(self):
		return self.__language

	@property
	def ymcreds_username(self):
		return self.__ymcreds_username

	@property
	def ymcreds_password(self):
		return self.__ymcreds_password
