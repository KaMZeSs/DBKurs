from asyncio.windows_events import NULL
from time import sleep
import requests
from requests_html import HTMLSession
from bs4 import BeautifulSoup as BS

def download(url, file_name):
    with open(file_name, "wb") as file:
        response = requests.get(url=url, headers=headers)
        file.write(response.content)


headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36 Edg/100.0.1185.44'}
last_page = 30
page = 0
with open('streets.txt', "w") as file:
    while page < last_page:
        page += 1
        print(page)
        # r = requests.get('https://city-address.ru/region-61_donetsk/all-street/page-' + str(page), headers=headers)
        r = requests.get('https://mapdata.ru/rostovskaya-oblast/rostov-na-donu/ulicy/stranica-' + str(page), headers=headers)
        html = BS(r.content, 'html.parser')

        all = html.find_all('div', class_='content-item')

        if (len(all)) :
            for el in all :
                a = el.find('a')
                file.write(a.text + '\n')
                print(a.text)
        sleep(1)
        print('\n\n\n\n')        
