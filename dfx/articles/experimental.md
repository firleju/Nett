# Experimental features

Experimental features are features that are not contained in the
official TOML specification. 

Experimental features have to be enabled via the TomlSettings
object. Otherwise they will not work.

You use experimental features at your own risk. At any time 
it is possible that an experimental feature is removed or 
changed completely independently from Nett's library version.

Most API will be surfaced via the `Experimental` sub namespace,
although sometimes it may be required to extend standard API
with experimental methods / properties. In such cases the 
API will do nothing if the feature flag is not enabled.
