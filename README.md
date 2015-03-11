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
<ul>
    <li>[Microsoft ASP.NET MVC](http://www.asp.net/mvc)</li>
    <li>[Microsoft ASP.NET WebApi](http://www.asp.net/web-api)</li>
    <li>[MongoDB](http://docs.mongodb.org/ecosystem/drivers/csharp/)</li>
    <li>[AutoMapper](http://automapper.org/)</li>
    <li>[NLog](http://nlog-project.org/)</li>
    <li>[Elmah](https://code.google.com/p/elmah/)</li>
</ul>


## Frontend: ##
<ul>
    <li>
        [KnockoutJS](http://knockoutjs.com/)
        <ul>
            <li>[Knockout Mapping plugin](http://knockoutjs.com/)</li>
        </ul>
    </li>
    <li>[Bootstrap](http://getbootstrap.com)</li>
    <li>
        [jQuery](http://jquery.com/)
        <ul>
            <li>[jQuery.blockUI](http://malsup.com/jquery/block/)</li>
            <li>[jQuery.cookie](https://github.com/carhartl/jquery-cookie)</li>
            <li>[Bootstrap Calendar](https://github.com/Serhioromano/bootstrap-calendar)</li>
        </ul>
    </li>
</ul>

Written in C-Sharp


# Todo #
## Educational purposes: ##
<ul>
    <li>Separe WebApi from WebApp project</li>
    <li>Add new WebApp project using AngularJS instead of KnockoutJS</li>
    <li>Test other more scalable databases like Cassandra or FoundationDB</li>
    <li>Use OWIN authentication</li>
</ul>


## Functionall: ##
<ul>
    <li>Make app offline friendly</li>
    <li>Add to and from datespan search under History</li>
    <li>Able to add short description for every Exercise</li>
    <li>...</li>
</ul>


## Contact ##
Twitter: @mmo_80<br>
Url: [forloop.se](http://forloop.se)