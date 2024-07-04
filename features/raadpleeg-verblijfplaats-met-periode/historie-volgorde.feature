#language: nl

@api @geen-protocollering @valideer-volgorde
Functionaliteit: verblijfplaatshistorie wordt aflopend geleverd

  Als consumer van verblijfplaatshistorie
  wil ik dat de verblijfplaatsen van een persoon in de gevraagde periode aflopend wordt geleverd
  zodat de meest recente verblijfplaats de eerste verblijfplaats is in de verblijfplaatsen lijst

  Implementatie: de gevonden verblijfplaatsen moeten op volg_nr oplopend worden gesorteerd.
  De actuele verblijfplaats heeft namelijk volg_nr 0

  Achtergrond:
    Gegeven adres 'A1' heeft de volgende gegevens
    | gemeentecode (92.10) | identificatiecode verblijfplaats (11.80) | straatnaam (11.10) | huisnummer (11.20) |
    | 0800                 | 0800010000000001                         | Teststraat         | 1                  |
    En adres 'A2' heeft de volgende gegevens
    | gemeentecode (92.10) | identificatiecode verblijfplaats (11.80) | straatnaam (11.10) | huisnummer (11.20) |
    | 0800                 | 0800010000000002                         | Andere Teststraat  | 2                  |
    En adres 'A3' heeft de volgende gegevens
    | gemeentecode (92.10) | identificatiecode verblijfplaats (11.80) | straatnaam (11.10) | huisnummer (11.20) |
    | 0800                 | 0800010000000003                         | Nog een Teststraat | 3                  |

  Scenario: actuele en historische verblijfplaatsen
    Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20210526                           |
    En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20231014                           |
    Als verblijfplaatshistorie wordt gezocht met de volgende parameters
    | naam                | waarde              |
    | type                | RaadpleegMetPeriode |
    | burgerservicenummer | 000000012           |
    | datumVan            | 2023-01-01          |
    | datumTot            | 2024-01-01          |
    Dan heeft de response verblijfplaatsen met de volgende gegevens
    | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat            | huisnummer | functieAdres.code | functieAdres.omschrijving |
    | 20231014                 |                                  | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | Andere Teststraat | 2          | W                 | woonadres                 |
    | 20210526                 | 20231014                         | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | Teststraat        | 1          | W                 | woonadres                 |

  Scenario: verblijfplaatsen in Nederland en in buitenland
    Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20210526                           |
    En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
    | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
    | 20220730                               | 6014                          |
    En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20231014                           |
    Als verblijfplaatshistorie wordt gezocht met de volgende parameters
    | naam                | waarde              |
    | type                | RaadpleegMetPeriode |
    | burgerservicenummer | 000000012           |
    | datumVan            | 2022-01-01          |
    | datumTot            | 2024-01-01          |
    Dan heeft de response verblijfplaatsen met de volgende gegevens
    | datumAanvangAdreshouding | datumAanvangAdresBuitenland | datumAanvangVolgendeAdreshouding | datumAanvangVolgendeAdresBuitenland | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | land.code | land.omschrijving            | straat            | huisnummer | functieAdres.code | functieAdres.omschrijving |
    | 20231014                 |                             |                                  |                                     | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 |           |                              | Andere Teststraat | 2          | W                 | woonadres                 |
    |                          | 20220730                    | 20231014                         |                                     |                              |                                      |                                  | 6014      | Verenigde Staten van Amerika |                   |            |                   |                           |
    | 20210526                 |                             |                                  | 20220730                            | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 |           |                              | Teststraat        | 1          | W                 | woonadres                 |

  Scenario: verblijfplaats in Nederland heeft datum aanvang met alleen jaar en maand bekend
    Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20210526                           |
    En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20231000                           |
    Als verblijfplaatshistorie wordt gezocht met de volgende parameters
    | naam                | waarde              |
    | type                | RaadpleegMetPeriode |
    | burgerservicenummer | 000000012           |
    | datumVan            | 2022-01-01          |
    | datumTot            | 2024-01-01          |
    Dan heeft de response verblijfplaatsen met de volgende gegevens
    | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | functieAdres.code | functieAdres.omschrijving | straat            | huisnummer |
    | 20231000                 |                                  | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | W                 | woonadres                 | Andere Teststraat | 2          |
    | 20210526                 | 20231000                         | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | W                 | woonadres                 | Teststraat        | 1          |

  Scenario: verblijfplaats in Nederland heeft datum aanvang met alleen jaar bekend
    Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20210526                           |
    En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20230000                           |
    Als verblijfplaatshistorie wordt gezocht met de volgende parameters
    | naam                | waarde              |
    | type                | RaadpleegMetPeriode |
    | burgerservicenummer | 000000012           |
    | datumVan            | 2022-01-01          |
    | datumTot            | 2024-01-01          |
    Dan heeft de response verblijfplaatsen met de volgende gegevens
    | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | functieAdres.code | functieAdres.omschrijving | straat            | huisnummer |
    | 20230000                 |                                  | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | W                 | woonadres                 | Andere Teststraat | 2          |
    | 20210526                 | 20230000                         | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | W                 | woonadres                 | Teststraat        | 1          |

  Scenario: verblijfplaats in Nederland heeft datum aanvang is onbekend
    Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20220526                           |
    En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 00000000                           |
    Als verblijfplaatshistorie wordt gezocht met de volgende parameters
    | naam                | waarde              |
    | type                | RaadpleegMetPeriode |
    | burgerservicenummer | 000000012           |
    | datumVan            | 2022-01-01          |
    | datumTot            | 2024-01-01          |
    Dan heeft de response verblijfplaatsen met de volgende gegevens
    | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | functieAdres.code | functieAdres.omschrijving | straat            | huisnummer |
    | 00000000                 |                                  | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | W                 | woonadres                 | Andere Teststraat | 2          |
    | 20220526                 | 00000000                         | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | W                 | woonadres                 | Teststraat        | 1          |

  Scenario: meerdere verblijfplaatsen in Nederland hebben datum aanvang is onbekend
    Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A3' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 00000000                           |
    En de persoon is vervolgens ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20220526                           |
    En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 00000000                           |
    Als verblijfplaatshistorie wordt gezocht met de volgende parameters
    | naam                | waarde              |
    | type                | RaadpleegMetPeriode |
    | burgerservicenummer | 000000012           |
    | datumVan            | 2022-01-01          |
    | datumTot            | 2024-01-01          |
    Dan heeft de response verblijfplaatsen met de volgende gegevens
    | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | functieAdres.code | functieAdres.omschrijving | straat             | huisnummer |
    | 00000000                 |                                  | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | W                 | woonadres                 | Andere Teststraat  | 2          |
    | 20220526                 | 00000000                         | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | W                 | woonadres                 | Teststraat         | 1          |
    | 00000000                 | 20220526                         | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000003                 | W                 | woonadres                 | Nog een Teststraat | 3          |

  Scenario: verblijfplaats buitenland heeft datum aanvang met alleen jaar en maand bekend
    Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20210526                           |
    En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
    | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
    | 20220700                               | 6014                          |
    En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20231014                           |
    Als verblijfplaatshistorie wordt gezocht met de volgende parameters
    | naam                | waarde              |
    | type                | RaadpleegMetPeriode |
    | burgerservicenummer | 000000012           |
    | datumVan            | 2022-01-01          |
    | datumTot            | 2024-01-01          |
    Dan heeft de response verblijfplaatsen met de volgende gegevens
    | datumAanvangAdreshouding | datumAanvangAdresBuitenland | datumAanvangVolgendeAdreshouding | datumAanvangVolgendeAdresBuitenland | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | land.code | land.omschrijving            | functieAdres.code | functieAdres.omschrijving | straat            | huisnummer |
    | 20231014                 |                             |                                  |                                     | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 |           |                              | W                 | woonadres                 | Andere Teststraat | 2          |
    |                          | 20220700                    | 20231014                         |                                     |                              |                                      |                                  | 6014      | Verenigde Staten van Amerika |                   |                           |                   |            |
    | 20210526                 |                             |                                  | 20220700                            | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 |           |                              | W                 | woonadres                 | Teststraat        | 1          |

  Scenario: verblijfplaats buitenland heeft datum aanvang met alleen jaar bekend
    Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20210526                           |
    En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
    | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
    | 20220000                               | 6014                          |
    En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20231014                           |
    Als verblijfplaatshistorie wordt gezocht met de volgende parameters
    | naam                | waarde              |
    | type                | RaadpleegMetPeriode |
    | burgerservicenummer | 000000012           |
    | datumVan            | 2021-12-31          |
    | datumTot            | 2024-01-01          |
    Dan heeft de response verblijfplaatsen met de volgende gegevens
    | datumAanvangAdreshouding | datumAanvangAdresBuitenland | datumAanvangVolgendeAdreshouding | datumAanvangVolgendeAdresBuitenland | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | land.code | land.omschrijving            | functieAdres.code | functieAdres.omschrijving | straat            | huisnummer |
    | 20231014                 |                             |                                  |                                     | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 |           |                              | W                 | woonadres                 | Andere Teststraat | 2          |
    |                          | 20220000                    | 20231014                         |                                     |                              |                                      |                                  | 6014      | Verenigde Staten van Amerika |                   |                           |                   |            |
    | 20210526                 |                             |                                  | 20220000                            | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 |           |                              | W                 | woonadres                 | Teststraat        | 1          |

  Scenario: verblijfplaats buitenland heeft datum aanvang is onbekend
    Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20210526                           |
    En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
    | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
    | 00000000                               | 6014                          |
    En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
    | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
    | 0800                              | 20231014                           |
    Als verblijfplaatshistorie wordt gezocht met de volgende parameters
    | naam                | waarde              |
    | type                | RaadpleegMetPeriode |
    | burgerservicenummer | 000000012           |
    | datumVan            | 2021-01-01          |
    | datumTot            | 2024-01-01          |
    Dan heeft de response verblijfplaatsen met de volgende gegevens
    | datumAanvangAdreshouding | datumAanvangAdresBuitenland | datumAanvangVolgendeAdreshouding | datumAanvangVolgendeAdresBuitenland | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | land.code | land.omschrijving            | functieAdres.code | functieAdres.omschrijving | straat            | huisnummer |
    | 20231014                 |                             |                                  |                                     | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 |           |                              | W                 | woonadres                 | Andere Teststraat | 2          |
    |                          | 00000000                    | 20231014                         |                                     |                              |                                      |                                  | 6014      | Verenigde Staten van Amerika |                   |                           |                   |            |
    | 20210526                 |                             |                                  | 00000000                            | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 |           |                              | W                 | woonadres                 | Teststraat        | 1          |
