[GET]
<h3>Получение всех пользователей:<h3/>
<h4><addres>/users<h4/>

[GET]
<h3>Получение пользователя по id:<h3/>
<h4>Пример:<h4/>
<h4><addres>/users/1<h4/>
  
[POST]  
<h3>Создание пользователя<h3/>
<h4><addres>/users/createUser<h4/>
  
<h4>
  Пример отправки данных:
{
  "login": "test5",
  "password": "test5",
  "last_name": "test5",
  "name": "test5",
  "middle_name": "test5",
  "is_admin": false,
  "class_group": "11-ГО",
  "sport": "Горный спорт"
}
  <h4/>
  
[GET]
Получить всю информацию по питанию
<addres>/food
  
[GET]
Получить информацию по питанию за день
Пример:
<addres>/food/bydate?year=2022&month=10&day=24
  
[GET]
Получить информацию по питанию по определённому студенту
Пример:
<addres>/food/bystudent?student_id=3
  
[GET]
Получение подробной Excel таблицы по каждому студенту об его питании на определенный день
Пример:
<addres>/food/document/getDocumentFoodNow?year=2022&month=10&day=24
  
[GET]
Получение Excel таблицы по кол-ву порций на сегодняшний день
Пример:
<addres>/food/document/getDocumentCountPersonFood?year=2022&month=10&day=24
  
[GET]
Получение Excel таблицы по кол-ву порций на текущий день
Пример:
<addres>/food/document/getDocumentFoodFoolMonth?year=2022&month=10
  
[POST]
Создание питания на день студента
<addres>/food/createFood
Пример:
{
  "student": 1,
  "date": "2022-10-24",
  "is_breakfast": true,
  "is_lunch": true,
  "is_after_lunch": false,
  "is_dinner": true,
  "is_breakfast_competition": false,
  "is_lunch_competition": true,
  "is_after_lunch_competition": false,
  "is_dinner_competition": false
}
  
[DELETE] 
Удаление питания на определенный день
<addres>/food/deleteFood
Пример:
{
  "year": 2022,
  "month": 10,
  "day": 24,
  "student_id": 1
}
  
