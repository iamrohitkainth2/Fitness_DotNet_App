// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
	const currentPath = window.location.pathname.toLowerCase().replace(/\/+$/, "");
	if (currentPath !== "/identity/account/login") {
		return;
	}

	const externalAccountSection = document.getElementById("external-account");
	if (externalAccountSection) {
		const externalColumn = externalAccountSection.closest("div[class*='col-']");
		if (externalColumn) {
			externalColumn.remove();
		} else {
			externalAccountSection.remove();
		}
	}

	const loginForm = document.getElementById("account");
	if (!loginForm) {
		return;
	}

	const accountColumn = loginForm.closest("div[class*='col-']");
	const loginRow = accountColumn ? accountColumn.parentElement : null;

	if (!accountColumn || !loginRow) {
		return;
	}

	const rowColumns = Array.from(loginRow.children).filter(function (element) {
		return element instanceof HTMLElement;
	});

	const externalSectionColumn = rowColumns.find(function (column) {
		if (column === accountColumn) {
			return false;
		}

		const heading = column.querySelector("h2, h3");
		return heading && heading.textContent && heading.textContent.trim().toLowerCase().startsWith("use another service to log in");
	});

	if (externalSectionColumn) {
		externalSectionColumn.remove();
	}

	loginRow.classList.add("justify-content-center");
	accountColumn.classList.remove("col-md-4", "col-md-6", "col-md-offset-2");
	accountColumn.classList.add("col-md-7", "col-lg-5");
});
