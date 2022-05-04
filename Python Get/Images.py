from asyncio import tasks
import requests
from requests_html import HTMLSession
from bs4 import BeautifulSoup as BS


def download(url, file_name):
    with open(file_name, "wb") as file:
        response = requests.get(url=url, headers=headers)
        file.write(response.content)

def process_page(page, counter) :
    r = requests.get('https://www.discogs.com/ru/search/?limit=' + str(limit) + '&type=release&sort=hot%2Cdesc&ev=em_tr&page=' + str(page), headers=headers)
    html = BS(r.content, 'html.parser')

    layout = html.find('div', class_='cards cards_layout_large')

    all = layout.find_all('img')

    for el in all:
        src = el['data-src']
        download(url=src, file_name=str(counter))
        counter += 1

limit = 100

headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36 Edg/100.0.1185.44'}

def main() :
    counter = 0
    last_page = 30
    page = 0
    while page < last_page:
        page += 1
        print(page)
        process_page(page=page, counter=counter)
        counter += limit
    print('complete')

        
main()
        
