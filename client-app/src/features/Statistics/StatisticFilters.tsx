import React, { useContext } from "react";
import { Form, Label, Button, Checkbox } from "semantic-ui-react";
import TextInput from "../../app/common/form/TextInput";
import { Form as FinalForm, Field } from "react-final-form";
import { RootStoreContext } from "../../app/stores/rootStore";
import SelectInput from "../../app/common/form/SelectInput";
import { filterOptions } from "../../app/common/sort/filters";
import { observer } from "mobx-react-lite";
import CheckboxInput from "../../app/common/form/CheckboxInput";

const StatisticFilters = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    setName,
    name,
    filterMoveState,
    setFilter,
    setOnlyIntactOtion,
    onlyIntactShips,
  } = rootStore.statisticStore;

  const handleFinalFormSubmit = (values: any) => {
    console.log(values);
    setName(values.name);
    setFilter(values.filter);
    setOnlyIntactOtion(values.OIOption);
  };

  return (
    <FinalForm
      onSubmit={handleFinalFormSubmit}
      render={({ handleSubmit }) => (
        <Form onSubmit={handleSubmit}>
          <Field
            name="name"
            component={TextInput}
            placeholder="name"
            defaultValue={name}
          />
          <Field
            component={SelectInput}
            options={filterOptions}
            name="filter"
            defaultValue={filterMoveState}
          />
         <label htmlFor = "OIOption">Only Intact Ships</label>
            <Field<boolean>
              name="OIOption"
              id="OIOption"
              type="checkbox"
              initialValue={onlyIntactShips}
              component={CheckboxInput}
            />
            <br/>
          <Button positive content="Filter" />
        </Form>
      )}
    />
  );
};

export default observer(StatisticFilters);
