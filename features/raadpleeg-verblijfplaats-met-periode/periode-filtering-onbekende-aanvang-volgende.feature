#language: nl

@api @geen-protocollering
Functionaliteit: test dat alleen verblijfplaatsen met onbekende datum aanvang die binnen de gevraagde periode vallen worden geleverd

    Achtergrond:
      Gegeven adres 'A1' heeft de volgende gegevens
      | gemeentecode (92.10) | identificatiecode verblijfplaats (11.80) | straatnaam (11.10) |
      | 0800                 | 0800010000000001                         | Eerste straat      |
      En adres 'A2' heeft de volgende gegevens
      | gemeentecode (92.10) | identificatiecode verblijfplaats (11.80) | straatnaam (11.10) |
      | 0800                 | 0800010000000002                         | Tweede straat      |
      En adres 'A3' heeft de volgende gegevens
      | gemeentecode (92.10) | identificatiecode verblijfplaats (11.80) | straatnaam (11.10) |
      | 0800                 | 0800010000000003                         | Derde straat       |
      En adres 'A4' heeft de volgende gegevens
      | gemeentecode (92.10) | identificatiecode verblijfplaats (11.80) | straatnaam (11.10) |
      | 0800                 | 0800010000000004                         | Vierde straat      |


  Regel: Een verblijfplaats met onbekende aanvang volgende verblijf wordt niet geleverd wanneer de gevraagde periode begint op of na de eerste dag van de onzekerheidsperiode van het volgende verblijf
         Dit is het geval wanneer de 1e dag van de onzekerheidsperiode volgende ≤ parameter datumVan
  
    Abstract Scenario: verblijf met aanvang adreshouding volgende verblijf <soort datum> wordt geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20080818                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      En de persoon is vervolgens ingeschreven op adres 'A3' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20161014                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | <datum van>         |
      | datumTot            | 2012-07-30          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | Tweede straat | <aanvang adreshouding>   | 20161014                         | W                 | woonadres                 |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | Eerste straat | 20080818                 | <aanvang adreshouding>           | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                                       | datum van  |
      | alleen jaar is bekend     | 20100000             | datumVan ligt voor de onzekerheidsperiode volgende                 | 2009-11-30 |
      | jaar en maand zijn bekend | 20100500             | datumVan ligt voor de onzekerheidsperiode volgende                 | 2010-02-19 |
      | alleen jaar is bekend     | 20100000             | datumVan is gelijk aan de dag voor de onzekerheidsperiode volgende | 2009-12-31 |
      | jaar en maand zijn bekend | 20100500             | datumVan is gelijk aan de dag voor de onzekerheidsperiode volgende | 2010-04-30 |
      | jaar en maand zijn bekend | 20100100             | datumVan is gelijk aan de dag voor de onzekerheidsperiode volgende | 2009-12-31 |
      | jaar en maand zijn bekend | 20100300             | datumVan is gelijk aan de dag voor de onzekerheidsperiode volgende | 2010-02-28 |
      | jaar en maand zijn bekend | 20120300             | datumVan is gelijk aan de dag voor de onzekerheidsperiode volgende | 2012-02-29 |

    Abstract Scenario: verblijf met aanvang adreshouding volgende verblijf <soort datum> wordt niet geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20080818                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      En de persoon is vervolgens ingeschreven op adres 'A3' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20161014                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | <datum van>         |
      | datumTot            | 2012-07-30          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | Tweede straat | <aanvang adreshouding>   | 20161014                         | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                                    | datum van  |
      | volledig onbekend         | 00000000             | datumVan ligt in de onzekerheidsperiode                         | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | datumVan ligt in de onzekerheidsperiode                         | 2010-05-26 |
      | jaar en maand zijn bekend | 20100500             | datumVan ligt in de onzekerheidsperiode                         | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | datumVan is gelijk aan de eerste dag van de onzekerheidsperiode | 2010-01-01 |
      | jaar en maand zijn bekend | 20100500             | datumVan is gelijk aan de eerste dag van de onzekerheidsperiode | 2010-05-01 |
      | alleen jaar is bekend     | 20100000             | datumVan is laatste dag van de onzekerheidsperiode              | 2010-12-31 |
      | jaar en maand zijn bekend | 20100500             | datumVan is laatste dag van de onzekerheidsperiode              | 2010-05-31 |
      | jaar en maand zijn bekend | 20101200             | datumVan is laatste dag van de onzekerheidsperiode              | 2010-12-31 |
      | jaar en maand zijn bekend | 20120200             | datumVan is laatste dag van de onzekerheidsperiode              | 2012-02-29 |
      | jaar en maand zijn bekend | 20100200             | datumVan is laatste dag van de onzekerheidsperiode              | 2010-02-28 |
      | alleen jaar is bekend     | 20100000             | datumVan ligt na de onzekerheidsperiode                         | 2011-02-19 |
      | alleen jaar is bekend     | 20100000             | datumVan is gelijk aan de eerste dag na de onzekerheidsperiode  | 2011-01-01 |
      | jaar en maand zijn bekend | 20100500             | datumVan ligt na de onzekerheidsperiode                         | 2011-02-19 |
      | jaar en maand zijn bekend | 20100500             | datumVan is gelijk aan de eerste dag na de onzekerheidsperiode  | 2010-06-01 |

    Abstract Scenario: verblijf wordt geleverd bij volgende is verblijf buitenland met aanvang <soort datum>, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20080818                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
      | <aanvang adreshouding>                 | 6014                          |
      En de persoon is vervolgens ingeschreven op adres 'A3' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20161014                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | <datum van>         |
      | datumTot            | 2012-07-30          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | land.code | land.omschrijving            | datumAanvangAdreshouding | datumAanvangAdresBuitenland | datumAanvangVolgendeAdreshouding | datumAanvangVolgendeAdresBuitenland | functieAdres.code | functieAdres.omschrijving |
      |                              |                                      |                                  |               | 6014      | Verenigde Staten van Amerika |                          | <aanvang adreshouding>      | 20161014                         |                                     |                   |                           |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | Eerste straat |           |                              | 20080818                 |                             |                                  | <aanvang adreshouding>              | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                                       | datum van  |
      | alleen jaar is bekend     | 20100000             | datumVan ligt voor de onzekerheidsperiode volgende                 | 2009-11-30 |
      | jaar en maand zijn bekend | 20100500             | datumVan ligt voor de onzekerheidsperiode volgende                 | 2010-02-19 |
      | alleen jaar is bekend     | 20100000             | datumVan is gelijk aan de dag voor de onzekerheidsperiode volgende | 2009-12-31 |
      | jaar en maand zijn bekend | 20100500             | datumVan is gelijk aan de dag voor de onzekerheidsperiode volgende | 2010-04-30 |
      | jaar en maand zijn bekend | 20100100             | datumVan is gelijk aan de dag voor de onzekerheidsperiode volgende | 2009-12-31 |
      | jaar en maand zijn bekend | 20100300             | datumVan is gelijk aan de dag voor de onzekerheidsperiode          | 2010-02-28 |
      | jaar en maand zijn bekend | 20120300             | datumVan is gelijk aan de dag voor de onzekerheidsperiode          | 2012-02-29 |

    Abstract Scenario: verblijf wordt niet geleverd bij volgende is verblijf buitenland met aanvang <soort datum>, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20080818                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
      | <aanvang adreshouding>                 | 6014                          |
      En de persoon is vervolgens ingeschreven op adres 'A3' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20161014                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde              |
      | type                | RaadpleegMetPeriode |
      | burgerservicenummer | 000000012           |
      | datumVan            | <datum van>         |
      | datumTot            | 2012-07-30          |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | land.code | land.omschrijving            | datumAanvangAdresBuitenland | datumAanvangVolgendeAdreshouding |
      | 6014      | Verenigde Staten van Amerika | <aanvang adreshouding>      | 20161014                         |
      
      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                                             | datum van  |
      | volledig onbekend         | 00000000             | datumVan ligt in de onzekerheidsperiode volgende                         | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | datumVan ligt in de onzekerheidsperiode volgende                         | 2010-05-26 |
      | jaar en maand zijn bekend | 20100500             | datumVan ligt in de onzekerheidsperiode volgende                         | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | datumVan is gelijk aan de eerste dag van de onzekerheidsperiode volgende | 2010-01-01 |
      | jaar en maand zijn bekend | 20100500             | datumVan is gelijk aan de eerste dag van de onzekerheidsperiode volgende | 2010-05-01 |
      | alleen jaar is bekend     | 20100000             | datumVan is laatste dag van de onzekerheidsperiode volgende              | 2010-12-31 |
      | jaar en maand zijn bekend | 20100500             | datumVan is laatste dag van de onzekerheidsperiode volgende              | 2010-05-31 |
      | jaar en maand zijn bekend | 20101200             | datumVan is laatste dag van de onzekerheidsperiode volgende              | 2010-12-31 |
      | jaar en maand zijn bekend | 20120200             | datumVan is laatste dag van de onzekerheidsperiode volgende              | 2012-02-29 |
      | jaar en maand zijn bekend | 20100200             | datumVan is laatste dag van de onzekerheidsperiode volgende              | 2010-02-28 |
      | alleen jaar is bekend     | 20100000             | datumVan ligt na de onzekerheidsperiode volgende                         | 2011-02-19 |
      | alleen jaar is bekend     | 20100000             | datumVan is gelijk aan de eerste dag na de onzekerheidsperiode volgende  | 2011-01-01 |
      | jaar en maand zijn bekend | 20100500             | datumVan ligt na de onzekerheidsperiode volgende                         | 2011-02-19 |
      | jaar en maand zijn bekend | 20100500             | datumVan is gelijk aan de eerste dag na de onzekerheidsperiode volgende  | 2010-06-01 |
