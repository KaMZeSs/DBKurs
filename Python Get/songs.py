from asyncio.windows_events import NULL
from email import header
from ntpath import join
import requests
from bs4 import BeautifulSoup as BS
import json
from joblib import Parallel, delayed
from selenium import webdriver
from selenium.webdriver.common.by import By
import random



# def process(i):
#     print(i)
#     try :
#         return json.loads(requests.get(url=url, headers=headers).text)['data']['name'] + '\n'
#     except :
#         return ''
#         pass
    


# url = 'https://story-shack-cdn-v2.glitch.me/generators/song-title-generator/[object%20PointerEvent]'
# headers = {
#     'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36 Edg/100.0.1185.44'}

# max = int(input())

# with open('result.txt', 'a') as file :
#     results = Parallel(n_jobs=30)(delayed(process)(i) for i in range(max))
    
#     file.writelines(results)

# url = 'https://www.coolgenerator.com/song-title-generator'
url = 'https://www.fantasynamegenerators.com/song-title-generator.php'
# values = ['hiphop', 'electronic', 'jazz',
        #   'country', 'blues', 'latin', 'pop', 'rock', 'rnb', 'ska']

driver = webdriver.Edge('C:\Program Files (x86)\Microsoft\Edge\Application\msedgedriver.exe')

driver.get(url)

max = int(input())

with open('songs.txt', 'w') as f :
    for i in range(max) :
        # driver.execute_script("document.getElementsByClassName('form-control')[0].value = '" + random.choice(values) + "'")
        driver.execute_script("nameGen()")
        a = driver.find_element(By.ID, 'result')
        # all = a.find_elements(By.TAG_NAME, 'span')
        f.write(a.text + '\n')
        # for el in all:
        #     s = str(el.text).strip()
        #     f.write(s + '\n')
        
driver.close()
quit()


        