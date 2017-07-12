# Important Note
> Project Created by **Top Nguyen** (http://topnguyen.net)

# Elastic Search Setup
- All table in database have view and elastic version exluce some table => view Elastic Exclude below.
     
- I not provide searchable or boost search for Order or User.
	- Because it very complicated and usually CUD in Business Logic.
    - **Security** issue (Elastic Search Engine) not provice anything to protect data.
    > So be careful: just index and "elastic" what is publish for end-user.
   - **No Transaction**
   - **Nearly realtime, not realtime**. => need call elastic **refresh to make it real time**

# Lessons
Elasticsearch is a great tool, but it is designed for search not to serve as a database

# Iso Code
## ISO 639-1 Language Codes
- https://www.w3schools.com/tags/ref_language_codes.asp
- http://www.loc.gov/standards/iso639-2/php/code_list.php

## Country Codes - ISO 3166
Focus: Vi by default + Support En
- https://www.iso.org/iso-3166-country-codes.html
- https://www.iso.org/obp/ui/#search
- https://countrycode.org/vietnam
- http://dialcode.org/Asia/Vietnam/
- https://en.wikipedia.org/wiki/ISO_3166-2:VN