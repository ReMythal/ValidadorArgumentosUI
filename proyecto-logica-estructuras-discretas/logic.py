import itertools
import re

def generar_combinaciones(n):
    """Genera combinaciones de True/False usando lógica binaria"""
    resultado = []
    total_filas = 2**n # Requisito: representar todas las combinaciones posibles [cite: 5, 23]
    
    for i in range(total_filas):
        # Convertimos el número a binario y rellenamos con ceros
        # '02b' si n=2 genera '00', '01', '10', '11'
        formato = '{:0' + str(n) + 'b}'
        binario = formato.format(i)
        
        # Convertimos '0' en True y '1' en False (o viceversa)
        # Nota: Generalmente en tablas de verdad se empieza con True
        fila = [bit == '0' for bit in binario]
        resultado.append(fila)
        
    return resultado

def limpiar_y_traducir(prop):
    # Tu lógica de limpieza y traducción [cite: 18]
    prop = prop.strip().lower()
    # Soporte para implicación si la llegan a usar [cite: 18]
    if "->" in prop:
        prop = re.sub(r'(\w+)\s*(?:->|→)\s*(\w+)', r'(not \1 or \2)', prop)
    prop_py = prop.replace("&&", " and ").replace("||", " or ").replace("!", " not ")
    return prop_py

def obtener_variables(formulas):
    # Tu lógica de extracción de variables [cite: 5, 20]
    texto = "".join(formulas)
    return sorted(list(set(c for c in texto if c.isalpha())))

def validar_argumento(premisas, conclusion):
    variables = obtener_variables(premisas + [conclusion])
    
    # SUSTITUCIÓN DE ITERTOOLS: Usamos tu nueva función 
    combinaciones = generar_combinaciones(len(variables))
    
    tabla_completa = []
    renglones_criticos = []
    es_valido = True

    for combo in combinaciones:
        contexto_valores = dict(zip(variables, combo))
        
        try:
            # Evaluar premisas usando tu método de eval [cite: 5, 7]
            valores_premisas = [eval(limpiar_y_traducir(p), {"__builtins__": None}, contexto_valores) for p in premisas]
            valor_conclusion = eval(limpiar_y_traducir(conclusion), {"__builtins__": None}, contexto_valores)
            
            fila = {**contexto_valores, "premisas": valores_premisas, "conclusion": valor_conclusion}
            tabla_completa.append(fila)

            # Identificar renglones críticos (Todas las premisas True) [cite: 6, 25]
            if all(valores_premisas):
                renglones_criticos.append(fila)
                # Si en un crítico la conclusión es False, es inválido [cite: 8, 29]
                if not valor_conclusion:
                    es_valido = False
        except Exception:
            return "Error de sintaxis", [], [], []

    return es_valido, tabla_completa, variables, renglones_criticos