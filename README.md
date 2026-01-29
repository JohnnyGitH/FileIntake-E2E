# 🧪 FileIntake-E2E

End-to-end (E2E) smoke tests for the **FileIntake** application.

This repo uses **Playwright for .NET** to run browser-based tests against the deployed app (primarily the **UAT** environment). The goal is to validate core user flows (site loads, auth works, file upload works, AI processing returns output) before promoting changes to production.

## 🔍 What this tests (initial scope)
- App is reachable (UAT)
- Key pages load without errors
- Basic UI elements exist (smoke tests)

## 🛠 Tech
- .NET 8
- Microsoft.Playwright
- NUnit

## ▶️ Running locally

1. Restore + build
```bash
dotnet restore
dotnet build
```