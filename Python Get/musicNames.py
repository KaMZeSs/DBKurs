from asyncio.windows_events import NULL
from email import header
from ntpath import join
import requests
from bs4 import BeautifulSoup as BS
import json
from joblib import Parallel, delayed
from selenium import webdriver
from selenium.webdriver.common.by import By



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

url = 'https://anytexteditor.com/ru/song-name-generator'

driver = webdriver.Edge('C:\Program Files (x86)\Microsoft\Edge\Application\msedgedriver.exe')

driver.get(url)

max = int(int(input()) / 10)

with open('result.txt', 'w') as f :
    for i in range(max) :
        driver.execute_script('nameGen()')
        s = str(driver.find_element(By.ID, 'result').text)
        f.write(s + '\n')
driver.close()
quit()


        