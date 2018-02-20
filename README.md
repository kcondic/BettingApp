# BettingApp
Short betting app with minimal functionalities in WebApi/Vue.js

Pri otvaranju projekta se pokreće webpack dev server na localhost:8080.
Ako se ne pokrene sam preko Task Runner Explorera, izvršit Node naredbu:
```
npm run dev
```
Za samo bundleanje/buildanje bez pokretanja servera:
```
npm run build
```
Ako izbacuje grešku oko block scoped deklaracija varijabli, napraviti iduće:
```
In the menu, go to Tools > Options > Projects and Solutions > Web Package Management > External Web Tools and DESELECT the option for $(VSINSTALLDIR)\Web\External.
```
