# Console.CDT_CryptoText

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.0-yellow.svg)]

Customer Data Typ der einen String verschlüsselt und wieder entschlüsselt zurück gibt.

Beispiele: Verschlüsseln, Entschlüsseln
```csharp
CryptoText txt1 = "Hallo Test-A";
CryptoText txt2 = "Hallo Test-B";

string verschluesseltTxt = txt1.ToString();

CryptoText txtEntschluesselt = verschluesselt;

CryptoText kopieTxt = txt1;
```

</br>
Beispiele: Vergleichen, Hashcode

```csharp
CryptoText txt1 = "Hallo Test-A";
CryptoText txt2 = "Hallo Test-B";

bool result = txt1 == txt2; // False

CryptoText kopieTxt = txt1;
bool result2 = kopieTxt == txt1; // True

long hashcode1 = txt1.GetHashCode();
long hashcode2 = kopieTxt.GetHashCode();

```

