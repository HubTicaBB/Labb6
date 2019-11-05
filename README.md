
Lab 6 - pub
Ni ska skapa en WPF-applikation som använder Task för att köra flera trådar. Använd GitHub för versionshantering. Laborationen ska utföras två och två.
Specifikation
WPF-applikationen ska simulera en pub. Puben har en bartender, en servitör/servitris och ett varierande antal besökare. Dessutom finns det en bar, ett mindre antal stolar och en hylla med glas. Tanken är att man ska kunna följa vad som händer i baren i realtid.

Ni ska ha trådar som
genererar nya besökare (objekt av klassen Bouncer genererar besökare)
en tråd per besökare (Patron)
kontrollerar bartendern (Bartender)
kontrollerar servitören/servitrisen (Waiter eller Waitress, välj en)

Bouncer, Patron, Bartender och Waitress kallas för agenter, eftersom de kan agera i vår simulering.
Så här skulle det kunna se ut i en bar, efter att en gäst kommit in och precis blivit serverad. (Obs om bilden skiljer sig från det som står i texten så är det texten som gäller)



Det ska finnas tre ListBox-element; ett för bartendern, ett för servitrisen och en som inkastaren delar med alla gäster. När något intressant händer någon av agenterna så ska det skrivas ut i motsvarande ListBox. Använd Items.Insert i stället för Items.Add för att lägga till nya textmeddelanden först i ListBox.
Button-elementen för pausa/fortsätt och panik är valfria. Men de kan vara användbara vid testningen.

Ni ska använda Thread.Sleep för att låta de olika trådarna vänta, så saker inte sker omedelbart. Använd någorlunda realistiska tider, för att det ska vara enklare att följa med i vad som händer. (Använd variabler för tiderna så kan ni enkelt ändra så simuleringen går snabbare eller långsammare.)

Först på varje rad som skrivs ut ska det vara ett ordningsnummer eller en tidsstämpel.

Det ska framgå hur lång tid som återstår tills baren stänger. (Tills det inte släpps in några nya kunder. Puben stänger när alla har gått hem.) Dessutom ska det framgå tydligt hur många gäster puben har, hur många glas det finns i hyllan och hur många lediga stolar det finns.

Använd trådsäkra collections som t.ex. BlockingCollection och ConcurrentQueue<T> för t.ex. hyllan och stolarna. Läs: http://dotnetpattern.com/csharp-blockingcollection 
Agenterna
Understrukna fraser innebär att när händelsen inträffar ska programmet lägga till en textsträng i motsvarande ListBox.
Bartender
Bartender-tråden ska vänta i baren tills en kund dyker upp. Så fort kunden kommer till baren går bartendern till hyllan och plockar ett glas. Om det inte finns något glas i hyllan så väntar bartendern tills det kommer tillbaka ett glas. Sedan häller bartendern upp öl till kunden och väntar på nästa.
Det tar tre sekunder att hämta ett glas och tre sekunder till att hälla upp öl.
När alla besökare har gått så går bartendern hem.
Waitress
Servitrisen ska plocka upp alla tomma glas som finns på borden. Sedan diskar hon glasen och ställer dem i hyllan.
Det tar tio sekunder att plocka glasen från borden och femton sekunder att diska dem.
När alla besökare har gått så går servitrisen hem.
Bouncer
Inkastaren släpper in kunder slumpvis, efter tre till tio sekunder. Inkastaren kontrollerar leg, så att alla i baren kan veta vad kunden heter. (Slumpa ett namn åt nya kunder från en lista) Inkastaren slutar släppa in nya kunder när baren stänger och går hem direkt.
Patron
Varje besökare kommer in i puben och går direkt till baren. Sedan väntar hen tills bartendern har gett hen en öl. Då letar besökaren efter en ledig stol. Om det inte finns några lediga stolar så väntar besökaren tills det dyker upp en. När besökaren satt sig ner så dricker besökaren upp ölen. När ölen är färdigdrucken lämnar besökaren baren.
Det tar en sekund att komma till baren, fyra sekunder att gå till ett bord, och mellan tjugo och trettio sekunder (slumpa) att dricka ölen.
Startvärden
Puben ska vara öppen i 120 sekunder. Det ska finnas 8 glas och 9 stolar.
Det är tillåtet att ändra parametrarna (tider, antal glas och stolar) för att kontrollera att laborationen fungerar.
Testning
Ni ska testköra er applikation med olika värden på parametrarna:
standardvärden på allt
det finns 20 glas, men bara 3 stolar
det finns 20 stolar, men bara 5 glas
gästerna stannar dubbelt så länge
servitrisen plockar glas och diskar dubbelt så fort
baren är öppen i 5 minuter (5*60 = 300 sekunder) eller längre
couples night - inkastaren släpper in två gäster varje gång i stället för en
det tar dubbelt så lång tid mellan det att inkastaren släpper in gäster, men efter de första 20 sekunderna släpper hen in en busslast på 15 gäster på samma gång som en engångshändelse
Bedömning
När ni är klara med laborationen ska ni se till att en annan grupp granskar er laboration. Det är ert eget ansvar att se till att just er laboration blir granskad. (Om det är svårt att hitta kodgranskare, så rekommenderar jag mutor i form av riktig öl) Ni ska dessutom granska en annan grupps kod
Inlämning via ithsdistans
Granskningsprotokollet ska lämnas in i som en fil eller i en zippad fil på ithsdistans
Granskningsprotokollet ska innehålla:
vilken grupps laboration ni har granskat
URL till GitHub-repot
minst tre saker som ni tycker att gruppen har gjort bra
en eller två saker som man hade kunnat göra annorlunda eller bättre
andra reflektioner
Uppgiften lämnas in på ithsdistans
Eftersom det är en gruppuppgift så lämnar en partner in koden
Den partner som inte lämnar in kod skall istället lämna in en kommentar. Till exempel
“Jobbar i grupp med Pontus Lindgren”


Kriterier för godkänt
programmet går att köra i alla åtta testfall utan att det kraschar
när puben stänger ska alla gäster ha gått hem och det ska vara samma antal glas och lediga stolar som när den öppnade
en tråd per agent
understrykna händelser ska läggas till först i rätt ListBox
alla besökare får dricka öl till slut
ni har granskat en annan grupps kod
godkänd demonstration för läraren

Kriterier för väl godkänt
väl godkänd demonstration för läraren
det går att ändra simuleringens hastighet i det grafiska gränssnittet
alla besökare betjänas i den ordning de anlände till baren
väl strukturerad, lättläst och tydlig kod
följer objektorienterade principer vid design av klasser


Drink responsibly!
