#language: nl

@api @geen-protocollering @valideer-volgorde
Functionaliteit: test dat raadplegen historie met periode bij een opeenvolgende verblijfplaatsen met gelijke of overlappende datum aanvang correct wordt geleverd

    Achtergrond:
      Gegeven adres 'A1' heeft de volgende gegevens
      | gemeentecode (92.10) | straatnaam (11.10) |
      | 0800                 | Korte straatnaam   |

  Regel: een verblijfplaats wordt niet geleverd wanneer de bekende datum aanvang volgende verblijfplaats gelijk is aan (of eerder) dan de datum aanvang van het verblijf

    Scenario: wel leveren wanneer de datum aanvang van de gewijzigde verblijfplaats later is dan de aanvang oorspronkelijke
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20020701                           |
      En adres 'A1' is op '20101014' geactualiseerd met de volgende gegevens
      | identificatiecode verblijfplaats (11.80) | naam openbare ruimte (11.15) | woonplaats (11.70) | identificatiecode verblijfplaats (11.80) | identificatiecode nummeraanduiding (11.90) |
      | 0800010000000002                         | Officiele straatnaam         | Testdorp           | 0800010000000002                         | 0800200000000002                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2010-01-01          |
      | datumTot            | 2011-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | straat           | naamOpenbareRuimte   | woonplaats | adresseerbaarObjectIdentificatie | nummeraanduidingIdentificatie | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20101014                 |                                  | Korte straatnaam | Officiele straatnaam | Testdorp   | 0800010000000002                 | 0800200000000002              | W                 | woonadres                 |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20020701                 | 20101014                         | Korte straatnaam |                      |            |                                  |                               | W                 | woonadres                 |

    Abstract Scenario: niet leveren wanneer de datum aanvang van de gewijzigde verblijfplaats is <omschrijving> de aanvang oorspronkelijke
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20020701                           |
      En adres 'A1' is op '<datum aanvang>' geactualiseerd met de volgende gegevens
      | identificatiecode verblijfplaats (11.80) | naam openbare ruimte (11.15) | woonplaats (11.70) | identificatiecode verblijfplaats (11.80) | identificatiecode nummeraanduiding (11.90) |
      | 0800010000000002                         | Officiele straatnaam         | Testdorp           | 0800010000000002                         | 0800200000000002                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2002-01-01          |
      | datumTot            | 2011-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | straat           | naamOpenbareRuimte   | woonplaats | adresseerbaarObjectIdentificatie | nummeraanduidingIdentificatie | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | <datum aanvang>          | Korte straatnaam | Officiele straatnaam | Testdorp   | 0800010000000002                 | 0800200000000002              | W                 | woonadres                 |

      Voorbeelden:
      | omschrijving | datum aanvang |
      | gelijk aan   | 20020701      |
      | eerder dan   | 20020628      |

    Scenario: wel leveren wanneer de datum aanvang na gecorrigeerde verblijfplaats later is dan de aanvang oorspronkelijke
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20020701                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
      | 20101014                               | 6014                          |
      En de inschrijving is vervolgens gecorrigeerd als een inschrijving op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20101014                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2010-01-01          |
      | datumTot            | 2011-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | straat           | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20101014                 |                                  | Korte straatnaam | W                 | woonadres                 |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20020701                 | 20101014                         | Korte straatnaam | W                 | woonadres                 |

    Abstract Scenario: niet leveren wanneer de datum aanvang na gecorrigeerde verblijfplaats <omschrijving> de aanvang oorspronkelijke
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20020701                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
      | 20101014                               | 6014                          |
      En de inschrijving is vervolgens gecorrigeerd als een inschrijving op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <datum aanvang>                    |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2002-01-01          |
      | datumTot            | 2011-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | straat           | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | <datum aanvang>          | Korte straatnaam | W                 | woonadres                 |

      Voorbeelden:
      | omschrijving | datum aanvang |
      | gelijk aan   | 20020701      |
      | eerder dan   | 20020628      |
