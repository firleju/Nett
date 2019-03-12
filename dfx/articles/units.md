# Units

The 'Unit' experimental feature is a simplified version 
of a feature discussed in [#603](https://github.com/toml-lang/toml/issues/603)

The unit is a TomlValue meta data property to ease the processing
of input data.

Currently the proposed approach to implement custom data types is
to format them as strings.

```toml
length = "2.4m"
velo = "1.3m/s"
```

But reading TOML files with a lot of strings, that are really 
some value with a unit can be hard.

A nicer way to write this when the feature is enabled is

```toml
length = 2.4(m)
velo = 1.3 ( m/s )
```

