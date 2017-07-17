![Logo](favicon.ico)
# Monkey.Data
> Project Created by [**Top Nguyen**](http://topnguyen.net)

- This project is **Data Repository** of System.
- This project contain **Interfaces and Entities**.

# Elastic Search Note
- Not provide searchable or boost search for User Info or some Touch Info.
	- Because it very complicated and usually change/edit in Logic.
    - **Security** issue (Elastic Search Engine) not provice anything to protect data (We already have solution but I still not welcome this).
      > So be careful: just index and "elastic" what is publish for end-user.
   - **No Transaction**
   - **Nearly realtime, not realtime**. => need call elastic **refresh to make it real time**

- Elasticsearch is a great tool, but it is designed for search **not to serve as a database**.

# Iso Code
- This is some standard if project have multiple currency and language.

## ISO 639-1 Language Codes
- https://www.w3schools.com/tags/ref_language_codes.asp
- http://www.loc.gov/standards/iso639-2/php/code_list.php

## Country Codes - ISO 3166
- https://www.iso.org/iso-3166-country-codes.html
- https://www.iso.org/obp/ui/#search
- https://countrycode.org/vietnam
- http://dialcode.org/Asia/Vietnam/
- https://en.wikipedia.org/wiki/ISO_3166-2:VN