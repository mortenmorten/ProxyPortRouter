# Proxy Port Router

An application for switching the connect address of a local `netsh interface portproxy`setup.

## Config

Create a file `entries.json` in `%ProgramData%\Proxy Port Router` like this:

```json
{
  "listenAddress": "127.0.0.11",
  "entries": [
    {
      "name": "None",
      "address": ""
    },
    {
      "name": "First",
      "address": "192.168.1.42"
    },
    {
      "name": "Second",
      "address": "192.168.2.42"
    }
  ]
}
```

## Sync slave

Add argument `--slave [hostname]` to the executable to sync the current entry to the slave host

## REST endpoint

The application host a REST endpoint on port 8080.
