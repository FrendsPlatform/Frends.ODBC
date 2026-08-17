# Changelog

## [3.1.0] - 2026-08-17
### Fixed
- The task now returns structured error details in `Result.Error` when `ThrowErrorOnFailure` is disabled, instead of only a plain error string.
- Added `ErrorMessageOnFailure` option to allow customising the error message thrown or returned on failure.

## [3.0.0] - 2026-02-18
### Added
- Added DataReaderWrapper class to encapsulate OdbcDataReader and avoid assembly reference issues when using DataReader output mode.
- Result now implements IAsyncDisposable and exposes a DisposeAsync method for async resource cleanup.

### Changed
- [Breaking change] Updated target framework from net6.0 to net8.0.

## [2.1.0] - 2025-03-25
### Changed
- Update packages:
  System.Data.Odbc        7.0.0  -> 9.0.3
  coverlet.collector      3.1.2  -> 6.0.4
  Microsoft.NET.Test.Sdk  17.1.0 -> 17.13.0
  MSTest.TestAdapter      2.2.8  -> 3.8.3
  MSTest.TestFramework    2.2.8  -> 3.8.3

## [2.0.0] - 2024-09-13
### Added
- Breaking change - Support for returning data as a DataReader
  - The task now has an option field `OutputType`

### Security
- Updated Newtonsoft.Json to 13.0.3

## [1.0.0] - 2023-05-24
### Added
- Initial implementation