Здравей Петър,

Както говорихме пращам задачката.

Виж дали имаш достъп до това репо
-	https://github.com/solay/tt 

За задачата ще ти трябва MS SQL сървър на който да има ДБ: tt – или както там си я кръстиш. 

Базата може да се върже като
-	Мдф файл
-	Или да си я създадеш на ПЦ-то със скрипт
И двата варианта могат да се намерят тук 
 

SQL Server developer download 
https://www.microsoft.com/en-us/sql-server/sql-server-downloads

Структурата е следната 
 
-	Properties - са динамични свойства. Винаги могат да се добавят нови
-	Brand – е събирателната да наречем категория (vf, otelo, dsl, cable). Всеки бранд има Х продукта.
-	Product – е тарифата 
o	ProductProperties – свойства за този продукт
-	Hardwae е излишен в случая за тази задача

В репо-то има два проекта
-	Api
-	Mvc

-	За теб е АПИ-то

 

И задачките (от табличката долу – 1 & 2 са найстухев, мен ме интересува т.3 )
-	т. 1 процедурата и вюто ги няма в ДБ, трябва да се създадат
-	т. 2 с миграцията, ако не си правил миграции по-добре не я прави
-	т. 3 е интересната. Статичните свойства са ИД + име. Динамичните са от таблиците Properties & ProductProperties.
o	/brand/export 
	Да връща цялата информация бранд/продукт/свойства както е показано долу (json).
	Свойствата могат да бъдат вложени, сиреч да имат под-свойства (ui/tooltip/body)
	Този експорт трябва да е динамичен/рекурсивен – сиреч ако приложението е стартирано и манипулираме ДБ-то то промените да се отразяват в резултата на ендпойнта.

 

Един скрииншот от работещ експорт
 

Ако имаме този изходен резултат за продукт с код: SM2L

{
              "ui": {
                             "extra": "1-24 Monat: 31,99 €",
                             "blister": "<div blister-action=\"SM2L\">blister body</div>",
                             "tooltip": {
                                           "body": "<div class=\"oe-tooltip\">some tooltip</div>"
                             },
                             "availability": {}
              },
              "id": "SM2L",
              "name": "Vodafone Smart L "
}

И добавим ново пропърти в ДБ за: root/product/ui
-	С име: samo
 

И добавим стойност за това пропърти и продукт SM2L (id: 95)
-	Стойност: Levski
 

То при рефреш на резулатата F5 or Postman:
{
              "ui": {
                             “samo”: “Levski”,
                             "extra": "1-24 Monat: 31,99 €",
                             "blister": "<div blister-action=\"SM2L\">blister body</div>",
                             "tooltip": {
                                           "body": "<div class=\"oe-tooltip\">some tooltip</div>"
                             },
                             "availability": {}
              },
              "id": "SM2L",
              "name": "Vodafone Smart L "
}

В примера по-горе ползвам собствена имплементация където се позиционирам директно на елемента UI. В решението може да се инклудва цялото дърво e.g.
{
              “root”:  {
                             “product”: {
"ui": {
                             “samo”: “Levski”,
                             "extra": "1-24 Monat: 31,99 €",
                             "blister": "<div blister-action=\"SM2L\">blister body</div>",
                             "tooltip": {
                                           "body": "<div class=\"oe-tooltip\">some tooltip</div>"
                             },
                             "availability": {}
              }
… other childs
}
… other childs
}
"name": "Vodafone Smart L ",
"id": "SM2L"
}


Ако имаш въпроси винаги можеш да ми пишеш по вайбър/skype или https://www.facebook.com/stefan.michev 

 

Поздрави Стефан

------------------------------------------------------------------------------------------------
 
light ds GmbH
 
Stefan Michev
Senior Developer
VoIP: +49 211 74958725
Mobil: +359 886 839 431
Stefan.Michev@lightds.de
http://www.lightds.de/

Amtsgericht Köln / HRB 73703
Geschäftsführer: Jens Seidel, Michael Gieselmann

Diese E-mail ist vertraulich und nur für den Gebrauch durch die Person oder das Unternehmen bestimmt, an die/das Sie gerichtet ist. 
Wenn Sie nicht der beabsichtigte Empfänger sind und Sie diese E-mail fälschlicherweise empfangen haben, wird jegliche Nutzung, 
Verbreitung, Versenden, Drucken oder Kopieren hiermit ausdrücklich verboten. Sie sollten dann mit dem Absender durch kurze Antwort 
in Verbindung treten und alle Informationen von Ihrem System löschen und zerstören. Alle Ansichten oder Meinungen, die dargestellt 
werden, sind nur die des Autors und stellen nicht notwendigerweise die der light ds GmbH dar. 


