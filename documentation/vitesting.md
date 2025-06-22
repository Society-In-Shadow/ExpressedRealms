# Vitesting Notes

## Vee Validate

With vee validate, if a field is optional, you more then likely will need to add ".nullable()" to it.  This will allow it
to not validate it as a required field.

For example, it will without it, it will treat an empty string "" as okay, but if you return a null, it will error out
and say that it's a required field