# DoWorkGym #
Keep track of your traning workouts. Fully functional webapplication. Also used for educational purposes.


## Before getting started ##
In order to get cookies working correctly complete the following steps. <br />
1. Add "local.trainingapp.net" to your windows host file and point to 127.0.0.1<br />
2. Create Virtual Directory in IIS (local.trainingapp.net)<br />
3. Add host name under Bindings in IIS


## Demo ##
Visit: [dowork.forloop.se](http://dowork.forloop.se/)


# Uses #
## Backend: ##
* [Microsoft ASP.NET MVC](http://www.asp.net/mvc)
* [Microsoft ASP.NET WebApi](http://www.asp.net/web-api)
* [MongoDB](http://docs.mongodb.org/ecosystem/drivers/csharp/)
* [AutoMapper](http://automapper.org/)
* [NLog](http://nlog-project.org/)
* [Elmah](https://code.google.com/p/elmah/)


## Frontend: ##
* [KnockoutJS](http://knockoutjs.com/)
 * [Knockout Mapping plugin](http://knockoutjs.com/)
* [Bootstrap](http://getbootstrap.com)
* [jQuery](http://jquery.com/)
 * [jQuery.blockUI](http://malsup.com/jquery/block/)
 * [jQuery.cookie](https://github.com/carhartl/jquery-cookie)
 * [Bootstrap Calendar](https://github.com/Serhioromano/bootstrap-calendar)

Written in C-Sharp


# Todo #
## Educational purposes: ##
* Separe WebApi from WebApp project
* Add new WebApp project using AngularJS instead of KnockoutJS
* Test other more scalable databases like Cassandra or FoundationDB
* Use OWIN authentication


## Functionall: ##
* Make app offline friendly
* Add to and from datespan search under History
* Able to add short description for every Exercise
* ...


## Contact ##
Twitter: @mmo_80<br>
Url: [forloop.se](http://forloop.se)