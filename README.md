# Evently

A modular, domain-driven backend service for managing events such as concerts, shows, and live performances.  
Evently is developed as part of my learning journey into **Modular Monolith architecture**, with a focus on domain boundaries, maintainability, and scalable internal design.

> [!WARNING]
> **Educational Use Only**
> This project is a personal learning exercise designed to explore architectural concepts. It is **not intended for production use** or real-world deployment.
> Please note that **contributions are not accepted** at this time.
---

## 📌 Overview

Evently is a sample application built to practice designing robust modular systems in C# using **.NET Core**, **Entity Framework Core**, and **PostgreSQL**.  
The project showcases modern backend design principles such as:

- Modular Monolith architecture
- Domain-Driven Design (DDD)
- Clean Architecture
- Internal events and module communication
- Strong separation of concerns
- Clear and explicit application boundaries

This repository also serves as a record of the project’s evolution, including architectural decisions, patterns implemented, and lessons learned.

---

## 🏗️ Architecture

Evently is structured as a **Modular Monolith**, where each module encapsulates its own:

- Domain model
- Business logic
- Data access layer
- API layer (if exposed)

Modules interact via well-defined internal events rather than shared models, enabling:

- Low coupling
- High cohesion
- Easier future extraction to microservices (if ever needed)
---

## 🛠️ Tech Stack

- C# / .NET Core 10
- Entity Framework Core
- PostgreSQL
- Dapper
- MediatR
- FluentValidation
- Docker