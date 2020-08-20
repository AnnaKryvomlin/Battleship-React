import React from "react";
import { FieldRenderProps } from "react-final-form";

type Props = FieldRenderProps<boolean, any>;

const CheckboxInput: React.FC<Props> = ({
  input: { value, ...input }
}: Props) => <input {...input} type="checkbox" />;

export default CheckboxInput;
