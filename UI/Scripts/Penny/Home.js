/*global $, PENNY */

PENNY.namespace("home");

PENNY.home = (function () {
	"use strict";

	var $datePicker,
		url,
		selector,
		currentEntries,
		setDateLabel,
		highlightDays,
		highlightEntries,
		setUpDatePicker,
		getMonthAndYear,
		saveEntry,
		loadEntry,
		search,
		loadSearchResult,
		ready;

	$datePicker = null;

	url = {
		saveEntry: PENNY.common.url("/Home/SaveEntry"),
		loadEntry: PENNY.common.url("/Home/LoadEntry"),
		search: PENNY.common.url("/Home/Search"),
		loadEntriesforMonth: PENNY.common.url("/Home/LoadEntriesForMonth")
	};

	selector = {
		datePicker: "#date-picker-div",
		dateLabel: "#date-label",
		saveButton: "#save-button",
		entry: "#entry-textarea",
		tags: "#tags-textbox",
		searchButton: "#search-button",
		searchBox: "#search-textbox",
		searchResultDiv: "#search-result-div",
		loadSearchResultButton: "._load-search-result-button",
		days: "td a"
	};

	currentEntries = [];

	setDateLabel = function (dateStr) {
		$(selector.dateLabel).text(dateStr);
	};

	highlightDays = function () {
		var $datePickerDiv = $(selector.datePicker),
			days = $(selector.days, $datePickerDiv);

		days.each(function () {
			var $aTag = $(this),
				text = $aTag.text();
			if ($.isNumeric(text) && ($.inArray(parseInt(text, 10), currentEntries) !== -1)) {
				$aTag.css("font-weight", "bold").css("color", "#9933FF");
			} else {
				$aTag.css("font-weight", "normal").css("color", "#000000");
			}
		});

	};

	highlightEntries = function (year, month) {
		PENNY.ajax.post({
			url: url.loadEntriesforMonth,
			success: function (data) {
				currentEntries = data;
				highlightDays();
			},
			data: {
				year: year,
				month: month
			}
		});
	};

	setUpDatePicker = function () {
		var dateFormat = "DD d MM yy";
		$datePicker = $(selector.datePicker);
		$datePicker.datepicker({
			dateFormat: dateFormat,
			onSelect: function (date) {
				setDateLabel(date);
				loadEntry(date);
			},
			onChangeMonthYear: highlightEntries
		});
	};

	getMonthAndYear = function(date) {
		if (date) {
			return {
				month: date.getMonth() + 1,
				year: date.getFullYear()
			};
		}
		return null;
	};

	saveEntry = function () {
		var date = $(selector.dateLabel).text(),
			entry = $(selector.entry).val(),
			tags = $(selector.tags).val();
		
		PENNY.ajax.post({
			url: url.saveEntry,
			success: function () {
				var selectedDate = getMonthAndYear($(selector.datePicker).datepicker("getDate"));
				if (selectedDate) {
					highlightEntries(selectedDate.year, selectedDate.month);
				}
			},
			data: {
				Date: date,
				Text: entry,
				Tags: tags
			}
		});
	};

	search = function () {
		PENNY.ajax.post({
			url: url.search,
			success: function (data) {
				$(selector.searchResultDiv).html(data);
			},
			data: {
				query: $(selector.searchBox).val()
			}
		});
	};

	loadEntry = function (date) {
		PENNY.ajax.post({
			url: url.loadEntry,
			success: function (data) {
				$(selector.entry).val(data.Text);
				$(selector.tags).val(data.Tags);
				highlightDays();
			},
			data: { Date: date }
		});
	};

	loadSearchResult = function ($button) {
		var date = $button.attr("data-penny-date");
		$(selector.datePicker).datepicker("setDate", date);
		$(selector.dateLabel).text(date);
		loadEntry(date);
	};

	ready = function () {
		var date;
		setUpDatePicker();
		$(selector.saveButton).click(saveEntry);
		$(selector.searchButton).click(search);
		$(selector.searchResultDiv).on("click", selector.loadSearchResultButton, function () {
			loadSearchResult($(this));
		});
		date = getMonthAndYear(new Date());
		highlightEntries(date.year, date.month);
	};

	return {
		ready: ready
	};
}());