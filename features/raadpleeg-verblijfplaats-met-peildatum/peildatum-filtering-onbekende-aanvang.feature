#language: nl

@api @geen-protocollering
Functionaliteit: test dat alleen de verblijfplaats(en) waar persoon verbleef op peildatum wordt geleverd bij (gedeeltelijk) onbekende datum aanvang

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


Regel: De verblijfplaats wordt niet geleverd wanneer gevraagde peildatum ligt voor de 1e dag van de onzekerheidsperiode van de aanvang
       Dit is het geval wanneer de 1e dag van de onzekerheidsperiode > parameter peildatum

    Abstract Scenario: verblijf met aanvang adreshouding <soort datum> wordt geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20080818                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | <peildatum>           |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | Tweede straat | <aanvang adreshouding>   |                                  | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                   | peildatum  |
      | volledig onbekend         | 00000000             | peildatum ligt in onzekerheidsperiode          | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | peildatum ligt in onzekerheidsperiode          | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | peildatum is eerste dag onzekerheidsperiode    | 2010-01-01 |
      | alleen jaar is bekend     | 20100000             | peildatum is eerste dag na onzekerheidsperiode | 2011-01-01 |
      | alleen jaar is bekend     | 20100000             | peildatum ligt na onzekerheidsperiode          | 2011-07-30 |
      | jaar en maand zijn bekend | 20100500             | peildatum ligt in onzekerheidsperiode          | 2010-05-26 |
      | jaar en maand zijn bekend | 20100500             | peildatum is eerste dag onzekerheidsperiode    | 2010-05-01 |
      | jaar en maand zijn bekend | 20100500             | peildatum is eerste dag na onzekerheidsperiode | 2010-06-01 |
      | jaar en maand zijn bekend | 20100500             | peildatum ligt na onzekerheidsperiode          | 2010-07-30 |

    Abstract Scenario: verblijf met aanvang adreshouding <soort datum> wordt niet geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20080818                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | <peildatum>           |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | Eerste straat | 20080818                 | <aanvang adreshouding>           | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                         | peildatum  |
      | alleen jaar is bekend     | 20100000             | peildatum ligt voor begin onzekerheidsperiode        | 2009-07-01 |
      | jaar en maand zijn bekend | 20100500             | peildatum ligt voor voor begin onzekerheidsperiode   | 2010-03-01 |
      | alleen jaar is bekend     | 20100000             | peildatum is dag voor eerste dag onzekerheidsperiode | 2009-12-31 |
      | jaar en maand zijn bekend | 20100500             | peildatum is dag voor eerste dag onzekerheidsperiode | 2010-04-30 |
      | jaar en maand zijn bekend | 20120300             | peildatum is dag voor eerste dag onzekerheidsperiode | 2012-02-29 |

    Abstract Scenario: verblijf buitenland met aanvang <soort datum> wordt geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20080818                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
      | <aanvang adreshouding>                 | 6014                          |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20160526                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | <peildatum>           |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | land.code | land.omschrijving            | datumAanvangAdresBuitenland | datumAanvangVolgendeAdreshouding |
      | 6014      | Verenigde Staten van Amerika | <aanvang adreshouding>      | 20160526                         |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                   | peildatum  |
      | volledig onbekend         | 00000000             | peildatum ligt in onzekerheidsperiode          | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | peildatum ligt in onzekerheidsperiode          | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | peildatum is eerste dag onzekerheidsperiode    | 2010-01-01 |
      | alleen jaar is bekend     | 20100000             | peildatum is eerste dag na onzekerheidsperiode | 2011-01-01 |
      | alleen jaar is bekend     | 20100000             | peildatum ligt na onzekerheidsperiode          | 2011-07-30 |
      | jaar en maand zijn bekend | 20100500             | peildatum ligt in onzekerheidsperiode          | 2010-05-26 |
      | jaar en maand zijn bekend | 20100500             | peildatum is eerste dag onzekerheidsperiode    | 2010-05-01 |
      | jaar en maand zijn bekend | 20100500             | peildatum is eerste dag na onzekerheidsperiode | 2010-06-01 |
      | jaar en maand zijn bekend | 20100500             | peildatum ligt na onzekerheidsperiode          | 2010-07-30 |

    Abstract Scenario: verblijf buitenland met aanvang <soort datum> wordt niet geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20080818                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
      | <aanvang adreshouding>                 | 6014                          |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20160526                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | <peildatum>           |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdresBuitenland | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | Eerste straat | 20080818                 | <aanvang adreshouding>              | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                         | peildatum  |
      | alleen jaar is bekend     | 20100000             | peildatum ligt voor begin onzekerheidsperiode        | 2009-07-01 |
      | jaar en maand zijn bekend | 20100500             | peildatum ligt voor voor begin onzekerheidsperiode   | 2010-03-01 |
      | alleen jaar is bekend     | 20100000             | peildatum is dag voor eerste dag onzekerheidsperiode | 2009-12-31 |
      | jaar en maand zijn bekend | 20100500             | peildatum is dag voor eerste dag onzekerheidsperiode | 2010-04-30 |


  Regel: Een verblijfplaats met onbekende aanvang van het verblijf en (bekende) aanvang vorige verblijf binnen de onzekerheidsperiode wordt niet geleverd wanneer de gevraagde peildatum ligt op of voor de aanvang vorige verblijf
         Dit is het geval wanneer de dag na aanvang vorige ≥ parameter peildatum

    Abstract Scenario: verblijf met aanvang adreshouding <soort datum> wordt geleverd, omdat er geen vorige verblijfplaats is
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | 2010-05-26            |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | Eerste straat | <aanvang adreshouding>   | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding |
      | volledig onbekend         | 00000000             |
      | alleen jaar is bekend     | 20100000             |
      | jaar en maand zijn bekend | 20100500             |

    Abstract Scenario: verblijf met aanvang adreshouding <soort datum> wordt geleverd, omdat de aanvang vorige verblijfplaats voor de onzekerheidsperiode ligt
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20080818                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | 2010-05-26            |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | Tweede straat | <aanvang adreshouding>   |                                  | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding |
      | alleen jaar is bekend     | 20100000             |
      | jaar en maand zijn bekend | 20100500             |

    Abstract Scenario: verblijf met aanvang adreshouding <soort datum> en (bekende) aanvang vorige in de onzekerheidsperiode wordt geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20100526                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      En de persoon is vervolgens ingeschreven op adres 'A3' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20161014                           |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | <peildatum>           |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | Tweede straat | <aanvang adreshouding>   | 20161014                         | W                 | woonadres                 |
      
      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                               | peildatum  |
      | volledig onbekend         | 00000000             | peildatum ligt in onzekerheidsperiode en na aanvang vorige | 2012-01-01 |
      | alleen jaar is bekend     | 20100000             | peildatum ligt in onzekerheidsperiode en na aanvang vorige | 2010-08-01 |
      | jaar en maand zijn bekend | 20100500             | peildatum ligt in onzekerheidsperiode en na aanvang vorige | 2010-05-29 |
      | alleen jaar is bekend     | 20100000             | peildatum is dag na aanvang vorige                         | 2010-05-27 |
      | jaar en maand zijn bekend | 20100500             | peildatum is dag na aanvang vorige                         | 2010-05-27 |
      | alleen jaar is bekend     | 20100000             | peildatum ligt na onzekerheidsperiode                      | 2011-03-14 |
      | jaar en maand zijn bekend | 20100500             | peildatum ligt na onzekerheidsperiode                      | 2010-07-01 |

    Abstract Scenario: verblijf met aanvang adreshouding <soort datum> en (bekende) aanvang vorige in de onzekerheidsperiode wordt niet geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20071014                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20100526                           |
      En de persoon is vervolgens ingeschreven op adres 'A3' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | <peildatum>           |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | Tweede straat | 20100526                 | <aanvang adreshouding>           | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                                 | peildatum  |
      | volledig onbekend         | 00000000             | peildatum is gelijk aan aanvang vorige                       | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | peildatum is gelijk aan aanvang vorige                       | 2010-05-26 |
      | jaar en maand zijn bekend | 20100500             | peildatum is gelijk aan aanvang vorige                       | 2010-05-26 |

    Abstract Scenario: verblijf met aanvang adreshouding <soort datum> en (bekende) aanvang vorige in de onzekerheidsperiode wordt niet geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20071014                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20100526                           |
      En de persoon is vervolgens ingeschreven op adres 'A3' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | <peildatum>           |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000001                 | Eerste straat | 20071014                 | 20100526                         | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                                 | peildatum  |
      | volledig onbekend         | 00000000             | peildatum ligt in onzekerheidsperiode en voor aanvang vorige | 2010-01-01 |
      | alleen jaar is bekend     | 20100000             | peildatum ligt in onzekerheidsperiode en voor aanvang vorige | 2010-04-01 |
      | jaar en maand zijn bekend | 20100500             | peildatum ligt in onzekerheidsperiode en voor aanvang vorige | 2010-05-10 |

    Abstract Scenario: verblijf buitenland met aanvang adreshouding <soort datum> en (bekende) aanvang vorige in de onzekerheidsperiode wordt niet geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20071014                           |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20100526                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
      | <aanvang adreshouding>                 | 6014                          |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | <peildatum>           |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | gemeenteVanInschrijving.code | gemeenteVanInschrijving.omschrijving | adresseerbaarObjectIdentificatie | straat        | datumAanvangAdreshouding | datumAanvangVolgendeAdreshouding | datumAanvangVolgendeAdresBuitenland | functieAdres.code | functieAdres.omschrijving |
      | 0800                         | Hoogeloon, Hapert en Casteren        | 0800010000000002                 | Tweede straat | 20100526                 |                                  | <aanvang adreshouding>              | W                 | woonadres                 |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                             | peildatum  |
      | volledig onbekend         | 00000000             | peildatum is datum aanvang vorige                        | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | peildatum is datum aanvang vorige                        | 2010-05-26 |
      | jaar en maand zijn bekend | 20100500             | peildatum is datum aanvang vorige                        | 2010-05-26 |

    Abstract Scenario: verblijf met aanvang adreshouding <soort datum> en (bekende) aanvang vorige verblijf buitenland in de onzekerheidsperiode wordt niet geleverd, omdat <omschrijving>
      Gegeven de persoon met burgerservicenummer '000000012' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | 20071014                           |
      En de persoon is vervolgens ingeschreven op een buitenlands adres met de volgende gegevens
      | datum aanvang adres buitenland (13.20) | land adres buitenland (13.10) |
      | 20100526                               | 6014                          |
      En de persoon is vervolgens ingeschreven op adres 'A2' met de volgende gegevens
      | gemeente van inschrijving (09.10) | datum aanvang adreshouding (10.30) |
      | 0800                              | <aanvang adreshouding>             |
      Als verblijfplaatshistorie wordt gezocht met de volgende parameters
      | naam                | waarde                |
      | type                | RaadpleegMetPeildatum |
      | burgerservicenummer | 000000012             |
      | peildatum           | <peildatum>           |
      Dan heeft de response verblijfplaatsen met de volgende gegevens
      | land.code | land.omschrijving            | datumAanvangAdresBuitenland | datumAanvangVolgendeAdreshouding |
      | 6014      | Verenigde Staten van Amerika | 20100526                    | <aanvang adreshouding>           |

      Voorbeelden:
      | soort datum               | aanvang adreshouding | omschrijving                                                 | peildatum  |
      | volledig onbekend         | 00000000             | peildatum is de dag na aanvang vorige                        | 2010-05-26 |
      | alleen jaar is bekend     | 20100000             | peildatum is de dag na aanvang vorige                        | 2010-05-26 |
      | jaar en maand zijn bekend | 20100500             | peildatum is de dag na aanvang vorige                        | 2010-05-26 |
