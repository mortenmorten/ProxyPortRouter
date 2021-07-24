# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Changed
- Upgrade to .NET 5

## [3.0.0] - 2018-06-13
### Added
- Added a service simulating a WebIO endpoint.

### Changed
- During testing I discovered that routing from i.e. 127.0.0.11 to 127.0.0.1 did not work. The listerAddress in entries.json should be changed to localhost. A change to MMAVAutomation is also needed.

## [2.1.1] - 2018-02-21
### Fixed
- Fixes the syncing process

## [2.1.0] - 2018-02-20
### Changed
- Web app will now poll the service every 5 seconds for changes to the current entry.

### Fixed
- Fixed hang in service when trying to update a dead slave

## [2.0.2] - 2018-02-20
### Added
- Enable syncing to slave service

## [2.0.0] - 2018-02-19
### Added
- Install using the supplied install.cmd script.

### Changed
- This version replaces the WPF controller with a web interface. The interface is hosted on port 8080.

## [1.0.0] - 2018-02-06
- Initial release
