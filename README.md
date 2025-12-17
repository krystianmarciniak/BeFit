BeFitUniMvc
Opis projektu

BeFitUniMvc to aplikacja webowa stworzona w technologii ASP.NET Core MVC, której celem jest ewidencja aktywności treningowej użytkowników.
Aplikacja umożliwia zarządzanie typami ćwiczeń, sesjami treningowymi oraz ćwiczeniami wykonywanymi w ramach sesji, z uwzględnieniem systemu użytkowników i ról.

Projekt został wykonany w ramach zajęć z przedmiotu Programowanie zaawansowane jako miniaplikacja wykorzystująca ORM, system użytkowników i ról oraz relacyjną bazę danych.

Zastosowane technologie

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (ORM)
- ASP.NET Core Identity (użytkownicy i role)
- SQL Server LocalDB
- Razor Views
- LINQ
- Visual Studio Code


Funkcjonalności aplikacji
## Zrzuty ekranu (działająca aplikacja)

Poniżej znajdują się przykładowe widoki aplikacji po uruchomieniu (lokalnie).  
Zrzuty mają pokazać kompletność funkcji CRUD oraz obecność statystyk użytkownika.

### Strona startowa (nawigacja)
![Witaj w BeFit](docs/screenshots/1_Witaj_w_BeFit.png)
- **Modele (3 wymagane):** ExerciseType, TrainingSession, PerformedExercise (walidacja + Display).


### Rodzaje ćwiczeń (publiczny podgląd + CRUD dla Admina)
![Rodzaje ćwiczeń](docs/screenshots/2_Rodzaje_cwiczen.png)
- **Typy ćwiczeń:** Index/Details publiczne, modyfikacje ograniczone do roli Admin.

### Sesje treningowe (CRUD tylko dla zalogowanego użytkownika)
![Sesje treningowe](docs/screenshots/3_Sesje_treningowe.png)
- **Sesje treningowe:** dostęp po zalogowaniu, rekordy przypisane do użytkownika, brak dostępu do cudzych danych.

### Wykonane ćwiczenia (CRUD tylko dla zalogowanego użytkownika, listy wyboru)
![Wykonane ćwiczenia](docs/screenshots/4_Wykonane_cwiczenia.png)
- **Wykonane ćwiczenia:** dostęp po zalogowaniu, wybór typu ćwiczenia i sesji z list (czytelne nazwy), ochrona własności danych.

### Statystyki (ostatnie 4 tygodnie / podsumowania użytkownika)
![Twoje statystyki](docs/screenshots/5_Twoje_statystyki.png)
- **Statystyki:** oddzielny widok z agregacją danych użytkownika z ostatnich 4 tygodni.

Modele danych

Aplikacja wykorzystuje trzy główne modele domenowe:

- ExerciseType – opisuje typ ćwiczenia,
- TrainingSession – reprezentuje sesję treningową użytkownika,
- PerformedExercise – łączy sesję treningową z typem ćwiczenia oraz przechowuje dane o obciążeniu, seriach i powtórzeniach.

Każdy model posiada walidację danych oraz czytelne etykiety wykorzystywane w widokach.

Bezpieczeństwo

- system logowania i rejestracji użytkowników oparty o ASP.NET Core Identity,
- role użytkowników: Admin, User,
- ograniczenie dostępu do danych wyłącznie do właściciela zasobu,
- operacje administracyjne dostępne tylko dla roli Admin.

Uruchomienie projektu

1. Sklonuj repozytorium:

git clone https://github.com/krystianmarciniak/BeFit

2. Przejdź do katalogu projektu:

cd BeFitUniMvc

3. Przygotuj bazę danych:

dotnet ef database update

4. Uruchom aplikację:

dotnet run

5. Aplikacja będzie dostępna pod adresem:

[text](http://localhost:5032/)

Konto administratora (seed)

Podczas uruchamiania aplikacji automatycznie tworzona jest rola administratora oraz konto testowe:

- e-mail: admin@befit.uni
- hasło: ZmienMnie!123

(hasło należy zmienić po pierwszym logowaniu)

Autor

Projekt wykonany samodzielnie w celach dydaktycznych.