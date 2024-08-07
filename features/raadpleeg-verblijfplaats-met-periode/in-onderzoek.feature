#language: nl

@api @geen-protocollering
Functionaliteit: leveren van inOnderzoek bij raadplegen met periode


    Achtergrond:
      Gegeven adres 'A1' heeft de volgende gegevens
      | gemeentecode (92.10) | straatnaam (11.10) | huisnummer (11.20) | postcode (11.60) |
      | 0800                 | Teststraat         | 1                  | 1234AB           |
      En adres 'A2' heeft de volgende gegevens
      | gemeentecode (92.10) | straatnaam (11.10) | huisnummer (11.20) | postcode (11.60) |
      | 0800                 | Teststraat         | 2                  | 1234CD           |


  Regel: in onderzoek gegevens op een geleverde verblijfplaats worden geleverd

    Scenario: In de periode heeft de persoon adres in Nederland met in onderzoek gegevens
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) |
      | 0800                              | 20021014                           | 81100                           | 20240501                       |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2006-01-01          |
      | datumTot            | 2007-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | straat     | huisnummer | postcode | inOnderzoek.aanduidingGegevensInOnderzoek | inOnderzoek.datumIngangOnderzoek | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20021014                 | Teststraat | 1          | 1234AB   | 081100                                    | 20240501                         | W                 | woonadres                 |

    Scenario: In de periode heeft de persoon verblijfplaats buitenland met in onderzoek gegevens
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20021014                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) | regel 1 adres buitenland (13.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) |
      | 1999                              | 20070516                               | 6014                          | Die Straße                       | 81300                           | 20240501                       |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2008-01-01          |
      | datumTot            | 2009-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | land.code | land.omschrijving            | regel1     | datumAanvangAdresBuitenland | inOnderzoek.aanduidingGegevensInOnderzoek | inOnderzoek.datumIngangOnderzoek |
      | 1999                         | Registratie Niet Ingezetenen (RNI)   | 6014      | Verenigde Staten van Amerika | Die Straße | 20070516                    | 081300                                    | 20240501                         |


  Regel: in onderzoek gegevens op de verblijfplaats chronologisch volgend op een geleverde verblijfplaats worden geleverd

    Scenario: In de periode heeft de persoon verblijfplaats en volgende verblijfplaats in Nederland heeft in onderzoek gegevens
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20021014                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) |
      | 0800                              | 20040526                           | 81100                           | 20240501                       |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2003-01-01          |
      | datumTot            | 2004-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | straat     | huisnummer | postcode | inOnderzoekVolgendeVerblijfplaats.aanduidingGegevensInOnderzoek | inOnderzoekVolgendeVerblijfplaats.datumIngangOnderzoek | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20021014                 | 20040526                         | Teststraat | 1          | 1234AB   | 081100                                                          | 20240501                                               | W                 | woonadres                 |

    Scenario: In de periode heeft de persoon verblijfplaats en beide hebben in onderzoek gegevens
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20021014                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) | regel 1 adres buitenland (13.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) |
      | 1999                              | 20070516                               | 6014                          | Die Straße                       | 81300                           | 20240501                       |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2006-01-01          |
      | datumTot            | 2007-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | datumAanvangVolgendeAdresBuitenland | straat     | huisnummer | postcode | inOnderzoekVolgendeVerblijfplaats.aanduidingGegevensInOnderzoek | inOnderzoekVolgendeVerblijfplaats.datumIngangOnderzoek | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20021014                 | 20070516                            | Teststraat | 1          | 1234AB   | 081300                                                          | 20240501                                               | W                 | woonadres                 |

    Scenario: In de periode heeft de persoon twee verblijfplaatsen en tweede verblijfplaats heeft in onderzoek gegevens
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) |
      | 0800                              | 20021014                           | 581100                          | 20240501                       |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) | regel 1 adres buitenland (13.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) |
      | 1999                              | 20070516                               | 6014                          | Die Straße                       | 81300                           | 20240502                       |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2007-01-01          |
      | datumTot            | 2008-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | datumAanvangAdresBuitenland | datumAanvangVolgendeAdresBuitenland | straat     | huisnummer | postcode | land.code | land.omschrijving            | regel1     | inOnderzoek.aanduidingGegevensInOnderzoek | inOnderzoek.datumIngangOnderzoek | inOnderzoekVolgendeVerblijfplaats.aanduidingGegevensInOnderzoek | inOnderzoekVolgendeVerblijfplaats.datumIngangOnderzoek | functieAdres.code | functieAdres.omschrijving |
      | 1999                         | Registratie Niet Ingezetenen (RNI)   |                          | 20070516                    |                                     |            |            |          | 6014      | Verenigde Staten van Amerika | Die Straße | 081300                                    | 20240502                         |                                                                 |                                                        |                   |                           |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20021014                 |                             | 20070516                            | Teststraat | 1          | 1234AB   |           |                              |            | 581100                                    | 20240501                         | 081300                                                          | 20240502                                               | W                 | woonadres                 |


  Regel: beëindigd onderzoek op een geleverde verblijfplaats wordt niet geleverd

    Scenario: In de periode heeft de persoon adres in Nederland met beëindigd onderzoek
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) | datum einde onderzoek (83.30) |
      | 0800                              | 20021014                           | 81100                           | 20240501                       | 20240529                      |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2006-01-01          |
      | datumTot            | 2007-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | straat     | huisnummer | postcode | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20021014                 | Teststraat | 1          | 1234AB   | W                 | woonadres                 |


  Regel: beëindigd onderzoek op een volgende verblijfplaats wordt niet geleverd

    Scenario: In de periode heeft de persoon verblijfplaats en volgende verblijfplaats in Nederland heeft beëindigd onderzoek 
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20021014                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) | datum einde onderzoek (83.30) |
      | 0800                              | 20040526                           | 081100                          | 20240501                       | 20240529                      |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2003-01-01          |
      | datumTot            | 2004-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | straat     | huisnummer | postcode | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20021014                 | 20040526                         | Teststraat | 1          | 1234AB   | W                 | woonadres                 |


  Regel: beëindigd onderzoek vastgesteld verblijft niet op adres wordt wel geleverd, inclusief datum einde

    Abstract Scenario: er is vastgesteld dat de persoon niet meer op het adres verblijft en het onderzoek is beëindigd
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) | datum einde onderzoek (83.30) |
      | 0800                              | 20211014                           | <aanduiding onderzoek>          | 20230516                       | 20230729                      |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20230730                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2023-01-01          |
      | datumTot            | 2024-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | straat     | huisnummer | postcode | functieAdres.code | functieAdres.omschrijving | inOnderzoek.aanduidingGegevensInOnderzoek | inOnderzoek.datumIngangOnderzoek | inOnderzoek.datumEindeOnderzoek |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20230730                 |                                  | Teststraat | 2          | 1234CD   | W                 | woonadres                 |                                           |                                  |                                 |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20211014                 | 20230730                         | Teststraat | 1          | 1234AB   | W                 | woonadres                 | <aanduiding onderzoek>                    | 20230516                         | 20230729                        |

      Voorbeelden:
      | aanduiding onderzoek |
      | 089999               |
      | 589999               |

    Abstract Scenario: beëindigd onderzoek vastgesteld verblijft niet op adres op volgende verblijfplaats wordt niet geleverd in inOnderzoekVolgendeVerblijfplaats
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20211014                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) | aanduiding in onderzoek (83.10) | datum ingang onderzoek (83.20) | datum einde onderzoek (83.30) |
      | 0800                              | 20230730                           | <aanduiding onderzoek>          | 20230516                       | 20230729                      |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | 2023-01-01          |
      | datumTot            | 2024-01-01          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | straat     | huisnummer | postcode | functieAdres.code | functieAdres.omschrijving | inOnderzoek.aanduidingGegevensInOnderzoek | inOnderzoek.datumIngangOnderzoek | inOnderzoek.datumEindeOnderzoek |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20230730                 |                                  | Teststraat | 2          | 1234CD   | W                 | woonadres                 | <aanduiding onderzoek>                    | 20230516                         | 20230729                        |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 20211014                 | 20230730                         | Teststraat | 1          | 1234AB   | W                 | woonadres                 |                                           |                                  |                                 |

      Voorbeelden:
      | aanduiding onderzoek |
      | 089999               |
      | 589999               |
