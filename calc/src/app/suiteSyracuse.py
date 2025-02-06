#Suite de syracuse
#nombre entier > 1, si pair on divise par 2, si impair on multiplie par 3 + 1
#doit se terminer par 4, 2 ou 1
    
# Calcule la suite de Syracuse pour un nombre donné
def syracuse(n: int) -> str:
    if n <= 0:
        return "Le nombre doit être un entier positif."
    
    sequence = [n]
    while n != 1:
        if n % 2 == 0: # Si le nombre est pair
            n //= 2 # Division entière par 2
        else:
            n = 3 * n + 1 # Multiplication par 3 et ajout de 1
        sequence.append(n)
    
    return ", ".join(map(str, sequence)) # Retourne la séquence sous forme de chaîne de caractères
