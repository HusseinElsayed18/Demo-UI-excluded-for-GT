# Decoupled UI & State Architecture for an ATM & Shopkeeper System

> "I designed and implemented a fully event-driven ATM and Shopkeeper transaction UI architecture from scratch in just 16 hours. Despite the rapid turnaround, I prioritized mobile-first optimization practices—such as state decoupling and data component caching—allowing the project to run at a rock-solid 120 FPS with minimal frame times."

A highly optimized, decoupled, and event-driven architecture built for a Unity-based ATM and Shopkeeper transaction simulator. This repository demonstrates how to isolate core economic logic, item banking, and player balance validations from the visual UI canvas layer using scalable C# patterns.

---

## ⚠️ Technical Focus Disclaimer (Mechanics & Optimization First)

* **Logic Over Art:** This repository is an exclusive demonstration of **core gameplay engineering, system architecture, and UI code decoupling**. No effort was placed on visual asset design or UI styling.
* **Production-Ready Foundations:** All UI panels use basic Unity layout shapes and fonts to strictly emphasize performance, data validation, and clean event execution states over aesthetic presentation.
* **Interchangeable Front-End:** Because the C# architecture is strictly decoupled, a professional UI/UX Artist could completely replace the visual graphics layer without altering a single line of underlying transaction logic.

---

## 🎯 Architectural Highlights
* **Strict Decoupling:** Shop mechanics, inventory lists, and banking databases do not hold rigid references to UI elements, ensuring the code remains testable and highly modular.
* **Event-Driven UI Updates:** Utilizes standard C# Actions and Delegates to trigger withdrawal screens, inventory state shifts, and wallet balances instantly without costly polling loops.
* **Performance-First Canvas Management:** Designed carefully to prevent constant Unity Canvas rebuilding spikes during quick multi-item menu toggles.

---

## 🎮 Gameplay & Interface Demo

[![Watch ATM & Shopkeeper Demo Video](https://github.com/user-attachments/assets/d19c443c-d766-461b-bed8-d28589e5cf7e)]

https://github.com/user-attachments/assets/d19c443c-d766-461b-bed8-d28589e5cf7e

*Having browser loading issues?* 
📂 [**Alternative Link: Access the Public Google Drive Folder**](https://drive.google.com/file/d/1-FlvOdiYCP1GV_RdBwszA7wLBTpQRZ50/view?usp=drive_link)

---

## 📁 Core C# Scripts & Technical Breakdown

Instead of digging through default subdirectories, you can click directly on the links below to evaluate my coding standards, design patterns, and performance handling:

### 1. 🏦 [ATM.cs](https://github.com)
* **Role in Architecture:** Handles banking transactions, processing card verification states, account balance deductions, and financial updates.
* **Code Implementation:** Manages numeric data transformations safely, ensuring synchronization between physical interface requests and player bank records.

### 2. 👥 [Player.cs](https://github.com)
* **Role in Architecture:** Acts as the central runtime entity data model, maintaining localized states for the user's wallet inventory balance, active currencies, and world collision interaction boundaries.
* **Optimization Strategy:** Implements clean parameter caching protocols, avoiding dynamic memory overhead during asset transfers or routine environment queries.

### 3. 🏪 [ShopKeeper.cs](https://github.com)
* **Role in Architecture:** Directs item matrix operations, controlling stock inventories, merchant interactions, and dynamic purchase/sell pricing metrics.
* **Architecture Pattern:** Uses decoupled event signals to inform the visual UI screens when transactions clear, avoiding monolithic updates.

### 4. 🔗 [Singletone.cs](https://github.com)
* **Role in Architecture:** A reusable structural base template providing global access points for scene systems while minimizing architectural dependencies.
* **Design Pattern:** Implements an optimized **Generic Singleton Pattern** framework to enforce secure instance allocations across managers.

---



