from logic import validar_argumento

print("--- PROYECTO: VALIDADOR DE ARGUMENTOS ---")

p = ["a || (b && c)"] 
c = "(a || b) && (a || c)"                  

valido, tabla, vars_nombres, criticos = validar_argumento(p, c)

if valido == "Error de sintaxis":
    print("Hubo un error en las fórmulas introducidas.") 
else:
    print(f"Variables: {vars_nombres}")
    print(f"Resultado: {'VÁLIDO' if valido else 'INVÁLIDO'}") 
    print(f"Se encontraron {len(criticos)} renglones críticos.")

    print("\nTabla de verdad completa:")
    for fila in tabla:
        print(fila)