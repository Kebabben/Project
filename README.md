# Project

Hämtar data från ResRobot och SMHI för att bestämma nästa resa (med kollektivtrafik) mellan Örebro C och en användarvald plats, och visar sedan avgågstid, ankomsttid och väder vid ankomsttiden.

Hur man använder:

Client och WebService måste vara markerade som startup project i Visual Studio. I Client måste sedan URL:en till WebService matas in, mitt fall så är denna adress "http://localhost:53697" (utan fnuttar). 
Sedan måste användaren välja en plats att resa till, detta görs i rutan till vänster om "Search". Den hämtar sedan alla platser som matchar den strängen. Så t.ex. om jag söker efter "hall" så kommer Hallsberg och Hallstahammar upp i listan. När användaren sedan klickar på något I listan så utför den själva sökningen och resultatet visas i klienten. 

WebService är en API, så den kan användas på följande sätt om man vill:

/weather/{lat}/{lon}/{dateTimeString}/

Returnerar XML med vädret vid angivna koordinater och vid angiven tidpunkt. dateTimeString ges i formatet yyyyMMddHHmm (år, månad, dag, timmar, minuter), t.ex. 201803091529.

/destination/{id}

Söker efter en plats baserat på dess indata och returnerar de platser som matchar söksträngen. T.ex. /destination/Hall

/trip/{id}

Söker efter en resa mellan Örebro Centralstation och den valda platsen. Sökningen från Örebro centralstation är hårdkodad och går endast att ändra i koden för WebService. Detta id är ett nummer som resrobots API tillhandahåller, denna används alltså inte för att söka med baserat på namnet av en plats.

/tripandweather/{id}

Den används inte av min klient, men den kan användas via webbläsare. id är namnet på en plats. WebService söker sedan efter id i resrobots API och tar den första som matchar och söker efter en resa från Örebro central station med den  strängen. Den ger också vädret vid ankomsttiden.
