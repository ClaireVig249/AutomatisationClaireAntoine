# Vérifie si un nombre est pair ou impair
def est_pair_ou_impair(nombre):
    if nombre % 2 == 0:
        return True
    else:
        return False

#Vérifie si un nombre est premier
def est_premier(nombre):
    if nombre <= 1:
        return False
    for i in range(2, int(nombre ** 0.5) + 1): # On teste jusqu'à la racine carrée de n
        if nombre % i == 0:
            return False
    return True

#Vérifie si un nombre est parfait
def est_parfait(nombre):
    if nombre < 1:
        return False
    somme_diviseurs = sum(i for i in range(1, nombre) if nombre % i == 0) # Somme des diviseurs
    if somme_diviseurs == nombre:
        return True
    else:
        return False
