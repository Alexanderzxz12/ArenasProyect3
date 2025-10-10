function MostrarModal(titulo = "Desea guardar los cambios?",
	texto = "Deseas registrar los cambios en la base de datos") {
	return Swal.fire({
		title: titulo,
		text: texto,
		icon: 'warning',
		showCancelButton: true,
		confirmButtonColor: '#3085d6',
		cancelButtonColor: '#d33',
		confirmButtonText: 'Si!'
	})
}