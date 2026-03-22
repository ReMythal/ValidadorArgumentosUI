import re

def generar_combinaciones(n): # Genera la tabla de verdad inicial de acuerdo a las proposiciones primitivas obtenidas
    resultado = []
    total_filas = 2**n 
    
    for i in range(total_filas): 
        formato = '{:0' + str(n) + 'b}' # Crea plantilla de para los datos binarios dentro de la tabla
        binario = formato.format(i)
        
        fila = [bit == '0' for bit in binario] # 0 = true, 1 = false (para mejor orden)
        resultado.append(fila)
        
    return resultado

def limpiar_y_traducir(prop): # Limpia espacios y traduce la sinttaxis para ser interpretada por eval
    prop = prop.strip().lower()
    if "->" in prop:
        prop = re.sub(r'(\w+)\s*(?:->|→)\s*(\w+)', r'(not \1 or \2)', prop)
    prop_py = prop.replace("&&", " and ").replace("||", " or ").replace("!", " not ")
    return prop_py

def obtener_variables(formulas): # Obtiene todas las proposiones primitivas
    texto = "".join(formulas)
    return sorted(list(set(c for c in texto if c.isalpha())))

def validar_argumento(premisas, conclusion):
    variables = obtener_variables(premisas + [conclusion])
    
    combinaciones = generar_combinaciones(len(variables))
    
    tabla_completa = []
    renglones_criticos = []
    es_valido = True

    for combo in combinaciones:
        contexto_valores = dict(zip(variables, combo))
        
        try:
            valores_premisas = [eval(limpiar_y_traducir(p), {"__builtins__": None}, contexto_valores) for p in premisas]
            valor_conclusion = eval(limpiar_y_traducir(conclusion), {"__builtins__": None}, contexto_valores)
            
            fila = {**contexto_valores, "premisas": valores_premisas, "conclusion": valor_conclusion}
            tabla_completa.append(fila)

            # Identifica renglones críticos 
            if all(valores_premisas):
                renglones_criticos.append(fila)
                
                if not valor_conclusion:
                    es_valido = False
        except Exception:
            return "Error de sintaxis", [], [], []

    return es_valido, tabla_completa, variables, renglones_criticos
    