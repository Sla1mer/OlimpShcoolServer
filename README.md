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
  Пример отправки данных: <br/>
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
<h3>Получить всю информацию по питанию<h3/>
<h4><addres>/food<h4/>
  
[GET]
<h3>Получить информацию по питанию за день<h3/>
<h4>Пример:<br/>
<addres>/food/bydate?year=2022&month=10&day=24<h4/>
  
[GET]
<h3>Получить информацию по питанию по определённому студенту<h3/>
<h4>Пример:<br/>
<addres>/food/bystudent?student_id=3<h4/>
  
[GET]
<h3>Получение подробной Excel таблицы по каждому студенту об его питании на определенный день<h3/>
<h4>Пример:<br/>
<addres>/food/document/getDocumentFoodNow?year=2022&month=10&day=24<h4/>
  
[GET]
<h3>Получение Excel таблицы по кол-ву порций на сегодняшний день<h3/>
<h4>Пример:<br/>
<addres>/food/document/getDocumentCountPersonFood?year=2022&month=10&day=24<h4/>
  
[GET]
<h3>Получение Excel таблицы по кол-ву порций на текущий день<h3/>
<h4>Пример:<br/>
<addres>/food/document/getDocumentFoodFoolMonth?year=2022&month=10<h4/>
  
[POST]
<h3>Создание питания на день студента<h3/>
<h4><addres>/food/createFood<br/>
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
}<h4/>
  
[DELETE] 
<h3>Удаление питания на определенный день<h3/>
<h4><addres>/food/deleteFood<br/>
Пример:<br/>
{
  "year": 2022,
  "month": 10,
  "day": 24,
  "student_id": 1
}<h4/>
  
