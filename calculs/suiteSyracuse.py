#Suite de syracuse
#nombre entier > 1, si pair on divise par 2, si impair on multiplie par 3 + 1
#doit se terminer par 4, 2 ou 1

def syracuse():
    global nombre
    tab = []
    while(nombre != 1):
        if(nombre % 2 == 0):
            nombre = nombre // 2
        else:
            nombre = nombre * 3 + 1
        tab.append(nombre)
    return tab[-1]
    print("Fin de la suite de Syracuse, nous avons parcouru : ", tab)



#Programme principal
nombre = 7

if(nombre > 1):
    result = syracuse()
    print("Le résultat est : ", result)
else:
    print("Le nombre doit être supérieur à 1")
