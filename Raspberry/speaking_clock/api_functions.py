# Модули
import requests
import json
from lxml import html


# Получение города по IP через DaData API
def get_dadata_city_by_ip(ip_address):

	# URL сервиса
	api_url = 'https://suggestions.dadata.ru/suggestions/api/4_1/rs/iplocate/address?ip=' + ip_address + '&language=ru'
	# API-ключ
	api_token = 'Token cb94fce6082599b4fc7b97c8826c344415a3c3ea'

	# Запрос к API
	api_result = requests.get( \
		api_url, \
		headers={'Accept': 'application/json', 'Authorization': api_token}).text

	# В результате должен быть основной объект location.
	# Вытаскиваем его
	api_json_result = json.loads(api_result)['location']

	# Если этот объект location существует в ответе сервера,
	# тогда вытаскиваем другие данные из JSON'а:
	if (api_json_result != None):

		# страна,
		country = api_json_result['data']['country']

		# тип населённого пункта (город, село),
		city_type = api_json_result['data']['city_type_full']

		# название населённого пункта.
		city_name = api_json_result['data']['city']

		# Текущее местоположение: Россия, город Ульяновск
		return f'Текущее местоположение: {country}, {city_type} {city_name}'

	return ''


# Склонение слов и словосочетаний (HtmlWeb)
def inflect(phrase, grammems=''):

	# Делаем запрос к API,
	# а список граммем добавляем только при их наличии
	inflect_result = requests.get(
		'https://htmlweb.ru/json/service/inflect?inflect=' +
		phrase + (('&grammems=' + grammems) if grammems != '' else ''))

	# Исключение OutOfRange, если сервер не вернул результат,
	# так как нулевого элемента в JSON-массиве items не будет
	return json.loads(inflect_result.content)['items'][0]

# Склонение чисел (Morpher 3.0).
# inflectCase - 1-ая буква требуемого падежа (И,Р,Д...)
# whatToReturn - что возвращать:
# 0=только число, 1=только единицу измерения,
# 2=всё вместе
def inflect_num(num, unit, inflectCase, whatToReturn=2):

	# Запрос к API
	inflect_result = requests.get(
		f'https://ws3.morpher.ru/russian/spell?n={num}&unit={unit}&format=json')

	inflect_json = json.loads(inflect_result)
	number_result = inflect_json['n'][inflectCase]
	unit_result = inflect_json['unit'][inflectCase]

	if (whatToReturn == 0):
		return number_result
	elif (whatToReturn == 1):
		return unit_result
	else:
		return f'{number_result} {unit_result}'


# Вспомогательная функция замены сокращений в рецептах
# для нормального прочтения Google TTS'ом
#
# Немного лингвистики:
# Название от приставки un- (означает обратное действие),
# глагол abbreviate (сократить, сделать аббревиатуру).
# По-русски, примерно, "разаббревиатурить" :)
#
def unabbreviate_recipe_item(item, correct_range):
	
	# Переменная для результата корректировок
	result = item

	# Проблемные сокращения
	# (Google произносит их совершенно неправильно)
	result = re.sub(r'ст\.\s*л\.', 'столовая ложка', result)
	result = re.sub(r'ч\.\s*л\.', 'чайная ложка', result)

	# Google произносит диапазон чисел (например, 1-2 ст.л.)
	# немного неверно: "Один - две столовых ложки".
	# Если нужно подкорректировать этот недочёт,
	# просто записываем цифры словами, с нужным родом
	numrange_regex = re.compile(r'1+?\s*\-\s*(\d+?)\s*((?:[А-Яа-я\.]+[\s]*){2})(.*)')
	numrange = numrange_regex.search(result)
	if (numrange != None):
		result = numrange_regex.sub(
			f'{inflect_num(1, numrange.group(2).strip(), "И", 0)}-' +
			f'{inflect_num(numrange.group(1), numrange.group(2).strip(), "И", 2)} ' +
			f'{numrange.group(3).strip()}')

	# Иногда зачем-то вместо обозначения
	# градусов Цельсия пихают русскую букву С
	result = re.sub(r'°[СC]', '°C', result)

# Функция для парсинга рецептов
# с сайта Gastronom.Ru (Еда.Ру и Поварёнок не подошли).
# query - поисковый запрос
def parse_recipe(query):

	# Загружаем страницу и вытаскиваем данные через LXML
	search_page = requests.get('https://www.gastronom.ru/search/type/recipe/?t='+query.strip())
	search_tree = html.fromstring(search_page.content)

	# Берём первый результат поиска:
	# тег article (первый результат = нулевой элемент массива HTML-элементов),
	# обёрнутый в блок с классами row и no-font-size (в нём располагаются все результаты)
	first_search_result = search_tree.xpath('//div[@class="row no-font-size"]//article[@class="material-anons col-sm-4 col-ms-6 "]')[0]
	# Вытаскиваем ссылку на этот первый рецепт (аттрибут href)
	search_result_link = first_search_result.xpath('.//a[@class="material-anons__title"]//@href')[0]

	# А теперь мучаем страничку с самим рецептом
	recipe_page = requests.get('https://www.gastronom.ru/'+search_result_link)
	recipe_tree = html.fromstring(recipe_page.content)

	# Сам рецепт у нас обёрнут в блок с огромным кол-вом классов,
	# который обёрнут в третий блок row (в массиве - 2ой элемент),
	# а row, как видите, в теге section
	all_recipe_page = recipe_tree.xpath('//section[@class="col-md-9 -border-left"]')[0]
	all_recipe = all_recipe_page.xpath('.//div[@class="row"]')[2].xpath('.//div[@class="col-md-8 col-sm-8 col-ms-8 wide-col"]')[0]

	# Строка для рецепта,
	# которую будет читать Google TTS
	recipe_text = ''

	# Сразу добавляем в неё название рецепта
	recipe_name = all_recipe_page.xpath('.//h1[@class="recipe__title"]')[0].text
	recipe_text += f'{recipe_name}. '

	# Ингредиенты.
	# Заголовков ("Для заливки","Для теста"...) несколько,
	# и их кол-во соответствует кол-ву списков под ними (тег ul),
	# поэтому это всё можно спокойно перебирать через цикл for
	ingredients_titles = all_recipe.xpath('.//div[@class="recipe__ingredient-title"]')
	ingredients_lists = all_recipe.xpath('.//ul')

	for index in range(len(ingredients_titles)):

		recipe_text += f'{ingredients_titles[index-1].text}: '

		ingredients_list_items = ingredients_lists[index-1].xpath('.//li[@class="recipe__ingredient"]')
		# li - list item
		for li in ingredients_list_items:
			# Записываем ингредиент в текст, предварительно
			# заменяя проблемные аббревиатуры (см. функцию выше)
			recipe_text += f'{unabbreviate_recipe_item(li.text)}; '

	# Приготовление.
	# Берём блок со всеми инструкциями
	recipe_instructions = all_recipe.xpath('.//div[@itemprop="recipeInstructions"]')[0]

	# Вытаскиваем заголовок ("Пошаговый рецепт приготовления")
	instructions_caption = recipe_instructions.xpath('.//h2[@class="recipe__ingredient-title"]')[0].text
	
	# Вытаскиваем сами "шаги" инструкции
	recipe_steps = recipe_instructions.xpath('.//div[@class="recipe__step"]')
	for step in recipe_steps:
		# Номер шага берём из его заголовка
		step_caption = step.xpath('.//div[@class="recipe__step-title"]')[0].text
		# Текст
		step_text = step.xpath('.//div[@class="recipe__step-text"]')[0].text
		# Добавляем в нашу строку,
		# на всякий случай корректируя всё той же функцией
		recipe_text += f'{step_caption}: {unabbreviate_recipe_item(step_text)}. '

	return recipe_text
