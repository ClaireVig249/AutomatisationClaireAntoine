def est_pair_ou_impair(nombre):
    if nombre % 2 == 0:
        return "pair"
    else:
        return "impair"

def est_premier(nombre):
    if nombre <= 1:
        return False
    for i in range(2, int(nombre ** 0.5) + 1):
        if nombre % i == 0:
            return False
    return True

def est_parfait(nombre):
    if nombre < 1:
        return False
    somme_diviseurs = sum(i for i in range(1, nombre) if nombre % i == 0)
    return somme_diviseurs == nombre


#Programme principal
nombre = 2
resultat = est_pair_ou_impair(nombre)
print(f"Le nombre {nombre} est {resultat}.")
print(f"Le nombre {nombre} est {'premier' if est_premier(nombre) else 'non premier'}.")
print(f"Le nombre {nombre} est {'parfait' if est_parfait(nombre) else 'non parfait'}.")