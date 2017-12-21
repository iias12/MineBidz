function addressDropdowns(ddlCountries, ddlStates) {
    ddlCountries.change(function () {
        var selectedCountryCode = $(this).val();
        if (!selectedCountryCode) {
            ddlStates.empty();
            ddlStates.append(
                    $('<option/>')
                         .attr('value', '')
                         .text('--Select Country First--'));
            return;
        }
        $.getJSON('@Url.Action("ProvincesStates", "Forms")', { countryCode: selectedCountryCode }, function (provinces) {
            ddlStates.empty();
            if (provinces.length > 0) {
                ddlStates.append(
                    $('<option/>')
                         .attr('value', '')
                         .text('--Please Select--')
                );
            }
            else {
                ddlStates.append(
                    $('<option/>')
                         .attr('value', 'NA')
                         .text('N/A')
                );
            }
            $.each(provinces, function (index, province) {
                ddlStates.append(
                        $('<option/>')
                            .attr('value', province.ProvinceStateCode)
                            .text(province.ProvinceStateName)
                );
            });
        });
    });
}
