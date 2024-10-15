# Automation in Digital Engineering Using Data Science

<p align="center">
  <img src="https://github.com/user-attachments/assets/56f2a626-8983-487c-8474-4df20a3cc2d1" alt="John Holland Group Logo" width="200" align="left"/>
  <img src="https://github.com/user-attachments/assets/757fa7de-166a-4c37-8ee8-c920876d53ab" alt="UniSA Logo" width="200" />
</p>


## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
  - [Plugin Installation](#plugin-installation)
  - [CLI Application Installation](#cli-application-installation)
- [Usage](#usage)
  - [Using the Navisworks Manage Plugin](#using-the-navisworks-manage-plugin)
  - [Using the Python CLI Application](#using-the-python-cli-application)
- [Documentation](#documentation)
- [Contributors](#contributors)
- [Acknowledgements](#acknowledgements)
- [Contact](#contact)
- [License](#license)

## Overview

**Automation in Digital Engineering Using Data Science** is a collaborative project developed by a team of four Master’s students from the University of South Australia, class of 2024, for the John Holland Group. This project serves as a demonstration of our innovative solution aimed at automating the **Level of Development (LoD) verification process** for **Building Information Modeling (BIM)** project management within the John Holland Group.

Our solution integrates a user-friendly **Navisworks Manage 2025 plugin** with a robust **Python-based CLI application** that implements advanced LoD verification algorithms. This combination streamlines workflows, enhances accuracy, and significantly reduces manual effort in BIM project management.

## Features

- **Navisworks Manage Plugin:**
  - Seamless integration with Navisworks Manage 2025.
  - User-friendly interface for initiating LoD verification.
  - Display the verification result into Navisworks.

- **Python CLI Application:**
  - Implements sophisticated LoD verification algorithms.
  - Command-line interface for advanced users and automation scripts.

## Prerequisites

To build, develop, or extend this project, ensure the following prerequisites are satisfied:

### General Requirements
**Navisworks Manage:** Version 2024 or compatible

### Development Requirements
1. **Visual Studio:** Required for building the Navisworks plugin (with VB support)
    -   .Net Framework 4.8 or compatible
    -   Navisworks SDK v22.0
2. **Python Packages:**
    -   pandas v2.2.3
#### Optional: for building the Python app
    - pyinstaller 6.10.0


## Installation

### Plugin Installation

1. **Download the Installer:**
   - Navigate to the [Releases](https://github.com/yourusername/your-repo/releases) page of this repository.
   - Download the latest `mysetup.exe` file.

2. **Run the Installer:**
   - Double-click the downloaded `mysetup.exe`.
   - Follow the on-screen instructions to install the plugin.
   - **Important:** Ensure you select the `Plugins` subfolder of your Navisworks Manage 2025 installation directory during the installation process.

3. **Verify Installation:**
   - Open **Navisworks Manage 2025**.
   - Navigate to the **Tool Add-ins** tab to confirm that the plugin is active.

## Usage

### Using the Navisworks Manage Plugin

1. **Launch Navisworks Manage 2025.**

2. **Access and Configure the Plugin:**
   - Click on the **Tool Add-ins** tab in the toolbar.
   - Select the **Unisa LoD Plugin** from the list of available plugins.
   - On **Setting** tab, select **Set AppData Folder** to select the folder for output. Ideally in your Document folder.

3. **Initiate LoD Verification:**
   - Select the model you wish to have verified.
   - Click on the **Setting** tab then **Run Verifyer** to start the process.
   - Upon finishing, click on the **Load Output** to select the newest output to display in Navisworks.

### Using the LoDVerifyer
**LoDVerifyer.exe** is a command-line tool that verifies the Level of Development (LoD) of BIM elements extracted from Navisworks models.
The application accepts two mandatory arguments:
1. **Input Path (`input`):**
   - **Type:** Path to a folder containing emails or unzipped subfolders.
   - **Description:** Specifies the location of the input data to be processed for LoD verification.
2. **Output Path (`output`):**
   - **Type:** Filepath for the output result.
   - **Description:** Defines where the verification results will be saved.

# Contributors

Thank you to all the contributors who have helped make this project a success!

## Core Team
- **Yongzhen Guan** – Project Manager
- **Grahi Nileshkumar Brahmbhatt** – Research & Design Verification Algorithm
- **Mahfuzul Islam Hemel** – Algorithm Design & Python Implementation
- **Nhat Minh Ngo** – Plugin Development & Integration

Special thanks to:
- **Yan Zepf** – Project Supervisor
- **Masud Karim** – Mentor
