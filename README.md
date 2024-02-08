# Zadatak 1

## ASP .NET CORE WEB API 7.0

### Napisati API upotrebom standardne biblioteke ili u frameworku po izboru koji ima sledece mogucnosti:
    * upload slike u folder
        * ako slika postoji odbijte upload razumnom porukom
        * naziv slike je SHA256(slike) tako da cemo onemoguciti duplikate
    * listanje uploadovanih slika gde je izlaz JSON output sa nazivima slika.
    * sortiraj nazive A-Z rastuce
    * brisanje odabrane slike, brise se po nazivu slike
    * download odabrane slike, preuzima se po nazivu slike
