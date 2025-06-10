CREATE TABLE OSIGURANIK(
id_osiguranika int PRIMARY KEY,
ime VARCHAR(50) NOT NULL,
prezime VARCHAR(50) NOT NULL,
JMBG int NOT NULL UNIQUE
)

CREATE TABLE SEKTOR(
id_sektora int primary key,
naziv_sektora VARCHAR(30) not null unique)

CREATE TABLE ADMINISTRATOR(
id_administratora int PRIMARY KEY,
ime VARCHAR(50) NOT NULL,
prezime VARCHAR(50) NOT NULL,
JMBG int NOT NULL UNIQUE,
sektor_id int not null,
FOREIGN KEY (sektor_id) REFERENCES SEKTOR(id_sektora)
)

CREATE TABLE LIKVIDATOR(
id_likvidatora int PRIMARY KEY,
ime VARCHAR(50) NOT NULL,
prezime VARCHAR(50) NOT NULL,
JMBG int NOT NULL UNIQUE,
sektor_id int not null,
FOREIGN KEY (sektor_id) REFERENCES SEKTOR(id_sektora)
)

CREATE TABLE TIP_OSIGURANJA(
id_tipa int Primary key,
naziv VARCHAR(50)
)

CREATE TABLE POLISA(
id_polise int Primary key,
datum_izdavanja DATE NOT NULL,
datum_isteka DATE,
tip_osiguranja int not null,
id_osiguranika int not null,
FOREIGN KEY (tip_osiguranja) REFERENCES TIP_OSIGURANJA(id_tipa),
FOREIGN KEY (id_osiguranika) REFERENCES OSIGURANIK (id_osiguranika)
)
CREATE TABLE STETA(
id_stete int primary key,
id_polise int not null,
id_osiguranika int not null,
id_likvidatora int not null,
id_administratora int not null,
lokacija VARCHAR(30) not null,
datum_prijave DATE not null,
datum_likvidacije DATE,
status VARCHAR(20) not null,
regres VARCHAR(2) NOT NULL,
FOREIGN KEY (id_osiguranika) REFERENCES OSIGURANIK (id_osiguranika),
FOREIGN KEY (id_polise) REFERENCES POLISA (id_polise),
FOREIGN KEY (id_likvidatora) REFERENCES LIKVIDATOR (id_likvidatora),
FOREIGN KEY (id_administratora) REFERENCES ADMINISTRATOR (id_administratora),
)
INSERT INTO OSIGURANIK(id_osiguranika, ime, prezime, JMBG)
VALUES
 (1, 'Ana', 'Petrović', 1203990123),
 (2, 'Marko', 'Jovanović', 010700078),
 (3, 'Ivana', 'Nikolić', 050600071),
 (4, 'Stefan', 'Lazić', 230400098),
 (5, 'Milica', 'Stojanović', 300199012);
INSERT INTO SEKTOR(id_sektora,naziv_sektora)
VALUES
(1,'vozila'),
(2,'lica'),
(3,'DZO'),
(4,'imovina'),
(5,'transport');
INSERT INTO ADMINISTRATOR(id_administratora,ime,prezime,JMBG,sektor_id)
VALUES
(1, 'Nikola', 'Ilić', 1203990, 2),
(2, 'Jelena', 'Marinković', 1070007, 1),
(3, 'Milan', 'Đorđević', 1504990, 3),
(4, 'Sara', 'Ristić', 2506990, 2),
(5, 'Lazar', 'Pavlović', 3010009, 1),
(6, 'Ivana', 'Petrović', 1506008, 3),
(7, 'Stefan', 'Jovanović', 2207991, 1),
(8, 'Marija', 'Nikolić', 3012990, 2),
(9, 'Aleksa', 'Kovačević', 1805009, 3),
(10, 'Teodora', 'Simić', 1102001, 1);
(11, 'Vuk', 'Bogdanović', 1208991, 4),
(12, 'Lana', 'Milić', 2703002, 5),
(13, 'Petar', 'Zorić', 0501993, 4),
(14, 'Nina', 'Đukić', 1307003, 5),
(15, 'Mihailo', 'Savić', 2212994, 4),
(16, 'Tamara', 'Vujić', 1010995, 5),
(17, 'Filip', 'Radovanović', 1904996, 4),
(18, 'Jovana', 'Milinković', 0808004, 5),
(19, 'Aleksa', 'Đorđević', 0910997, 4),
(20, 'Katarina', 'Rakić', 1503005, 5);
INSERT INTO TIP_OSIGURANJA(id_tipa, naziv)
VALUES
(1, 'Auto osiguranje'),
(2, 'Zdravstveno osiguranje'),
(3, 'Životno osiguranje'),
(4, 'Osiguranje imovine'),
(5, 'Putno osiguranje');
INSERT INTO LIKVIDATOR(id_likvidatora, ime, prezime, JMBG, sektor_id)
VALUES
(1, 'Milan', 'Jankovic', 1201987, 1),
(2, 'Tijana', 'Stankovic', 2507991, 3),
(3, 'Nemanja', 'Miloševic', 3012000, 2),
(4, 'Katarina', 'Pantic', 1504992, 5),
(5, 'Filip', 'Rakic', 1808003, 4),
(6, 'Jovana', 'Bogdanovic', 1106995, 2),
(7, 'Vladimir', 'Živkovic', 2303001, 1),
(8, 'Natalija', 'Radovanovic', 1701004, 5),
(9, 'Stefan', 'Vasic', 2609990, 3),
(10, 'Milena', 'Obradovic', 1402002, 4);
