![Logo](../../favicon.ico)
# Authorize by Attribute

> Project Created by [**Top Nguyen**](http://topnguyen.net)
- Support `[AllowAnonymous]` in `Action` and `Controller`.
- Support multiple attribute and multiple permissions with Permission Enums `[Authorize(Enums.Permission)]`.
- If multiple Authorize Attribute setup from `Controller` and `Action` then conditional `OR` will be apply.
- Support `Action` clear authorization setup by `Controller` via `[OverrideAuthorize]`.